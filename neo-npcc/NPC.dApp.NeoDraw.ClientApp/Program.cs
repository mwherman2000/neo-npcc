using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NPC.dApp.NeoDraw.ClientApp
{
    static class Constants
    {
        public const int NROWS = 40;
        public const int NCOLS = 40;
        public const string BAM = "BAM!";
        public static readonly char[] BAMCHAR = Constants.BAM.ToCharArray();
    }

    class BoardRow
    {
        public char[] Columns = new char[Constants.NCOLS];
    }

    static class BoardContext
    { 
        public static BoardRow[] Rows = new BoardRow[Constants.NROWS];

        public static void Initialize()
        {
            for (int irow = 0; irow < Constants.NROWS; irow++)
            {
                BoardRow row = new BoardRow();
                for (int icol = 0; icol < Constants.NCOLS; icol++)
                {
                    row.Columns[icol] = ' ';
                }
                Rows[irow] = row;
            }
        }

        public static void DrawBoard(string message)
        {
            Program.DisplaySplash(message);
            Console.Write("   "); for (int icol = 0; icol < Constants.NCOLS*2 + 3; icol++) Console.Write("-"); Console.WriteLine();
            for (int irow = 0; irow < Constants.NROWS; irow++)
            {
                Console.Write("   "); Console.Write("|");
                for (int icol = 0; icol < Constants.NCOLS; icol++)
                {
                    switch (Rows[irow].Columns[icol])
                    {
                        case '.':
                            {
                                ConsoleColor saved = Console.ForegroundColor;
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write(" " + Rows[irow].Columns[icol]);
                                Console.ForegroundColor = saved;
                                    break;
                            }
                        case '*':
                            {
                                ConsoleColor saved = Console.ForegroundColor;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(" " + Rows[irow].Columns[icol]);
                                Console.ForegroundColor = saved;
                                break;
                            }
                        default:
                            {
                                ConsoleColor saved = Console.ForegroundColor;
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(" " + Rows[irow].Columns[icol]);
                                Console.ForegroundColor = saved;
                                break;
                            }
                    }
                }
                Console.Write(" |");
                Console.WriteLine();
            }
            Console.Write("   "); for (int icol = 0; icol < Constants.NCOLS*2 + 3; icol++) Console.Write("-"); Console.WriteLine();
            Console.WriteLine();
        }

        internal static void DrawPoints(List<Point> pointList, char ch)
        {
            foreach(Point p in pointList)
            {
                if (p.X < 0 || p.X >= Constants.NCOLS) break;
                if (p.Y < 0 || p.Y >= Constants.NROWS) break;
                if (Rows[p.Y].Columns[p.X] != ' ')
                {
                    int xleft = p.X + 1;
                    int xright = p.X + Constants.BAM.Length;
                    if (xright >= (Constants.NCOLS - 1)) xleft = p.X - Constants.BAM.Length;
                    for (int xoffset = 0; xoffset < Constants.BAM.Length; xoffset++)
                    {
                        Rows[p.Y].Columns[xleft+xoffset] = Constants.BAM.ToCharArray()[xoffset];
                    }
                }
                Rows[p.Y].Columns[p.X] = ch;
            }
        }
    }

    class Program
    {
        const string ProgramName = "NeoDraw - the NEO Blockchain Drawing dApp";
        const string helpMessage = "Enter add, delete, get, exit, or help";

        static void Main(string[] args)
        {
            bool notdone = true;
            string username = "";
            string password = "";
            string command = "";
            string secretPhrase = "";
            string message = "Welcome to " + ProgramName;

            while (notdone)
            {
                DisplaySplash(message);

                Console.WriteLine(" Using the Properties dialog on the top-left menu:");
                Console.WriteLine(" 1. On the Fonts tab, set your Font Size to 12");
                Console.WriteLine(" 2. On the Layouts tab, set your Window Height and Width to 66 rows x 66 columns (minimum)");
                Console.WriteLine();
                Console.Write("Username: ");
                username = Console.ReadLine();

                // Reference: https://stackoverflow.com/questions/3404421/password-masking-console-application
                password = "";
                Console.Write("Password: ");
                ConsoleKeyInfo key;
                do
                {
                    key = Console.ReadKey(true);
                    if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                    {
                        password += key.KeyChar;
                        Console.Write("*");
                    }
                    else
                    {
                        if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                        {
                            password = password.Substring(0, (password.Length - 1));
                            Console.Write("\b \b");
                        }
                    }
                }
                while (key.Key != ConsoleKey.Enter);
                Console.WriteLine();

                // Reference: https://stackoverflow.com/questions/3404421/password-masking-console-application
                secretPhrase = "";
                Console.Write("Secret Phrase: ");
                do
                {
                    key = Console.ReadKey(true);
                    if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                    {
                        secretPhrase += key.KeyChar;
                        Console.Write("*");
                    }
                    else
                    {
                        if (key.Key == ConsoleKey.Backspace && secretPhrase.Length > 0)
                        {
                            secretPhrase = secretPhrase.Substring(0, (secretPhrase.Length - 1));
                            Console.Write("\b \b");
                        }
                    }
                }
                while (key.Key != ConsoleKey.Enter);
                Console.WriteLine();

                if (username.Length >= 1 && password.Length >= 1 && secretPhrase.Length >= 1) notdone = false;
            }

            byte[] hashUsername = Helpers.GetHash(username);
            byte[] hashPassword = Helpers.GetHash(username + password + secretPhrase);
            password = "";
            secretPhrase = "";

            BoardContext.Initialize();

            List<Point> pointList = Bresenham.GetLinePoints(new Point(2, 3), new Point(30, 5));
            BoardContext.DrawPoints(pointList, '*');
            pointList = Bresenham.GetLinePoints(new Point(20, 2), new Point(40,100));
            BoardContext.DrawPoints(pointList, '.');

            notdone = true;
            message = "Welcome " + username + "\t(" + BitConverter.ToString(hashUsername).Replace("-", "") + ")";
            while (notdone)
            {
                BoardContext.DrawBoard(message);

                Console.Write("Commmand: ");
                command = Console.ReadLine();
                string[] parts = command.Split(' ');
                if (parts.Length == 0)
                {
                    message = helpMessage;
                    continue;
                }
                string verb = parts[0];
                switch(verb)
                {
                    case "add":
                        {
                            break;
                        }
                    case "delete":
                        {
                            break;
                        }
                    case "get":
                        {
                            break;
                        }
                    case "exit":
                        {
                            notdone = false;
                            break;
                        }
                    case "help":
                        {
                            message = helpMessage;
                            break;
                        }
                    default:
                        {
                            message = helpMessage;
                            break;
                        }
                }
            }
            Console.WriteLine();
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        public static void DisplaySplash(string message)
        {
            Console.Clear();
            if (Trace.Splash) { for (int icol = 0; icol < Constants.NCOLS * 2 + 6; icol++) Console.Write("*"); Console.WriteLine(); }
            if (Trace.Splash) Console.WriteLine(" " + ProgramName + " v" + Assembly.GetEntryAssembly().GetName().Version.ToString());
            if (Trace.Splash) { for (int icol = 0; icol < Constants.NCOLS * 2 + 6; icol++) Console.Write("*"); Console.WriteLine(); }
            if (!String.IsNullOrEmpty(message))
            {
                Console.Write(" ");  Console.WriteLine(message);
                Console.WriteLine();
            }
        }
    }
}
