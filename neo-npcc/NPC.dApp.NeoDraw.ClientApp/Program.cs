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
                    Console.Write(" " + Rows[irow].Columns[icol]);
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

                if (username.Length >= 1 && password.Length >= 1) notdone = false;
            }

            byte[] hashUsername = Helpers.GetHash(username);
            byte[] hashPassword = Helpers.GetHash(password);

            BoardContext.Initialize();
            List<Point> pointList = Bresenham.GetLinePoints(new Point(2, 3), new Point(30, 5));
            BoardContext.DrawPoints(pointList, '*');
            pointList = Bresenham.GetLinePoints(new Point(20, 2), new Point(40,100));
            BoardContext.DrawPoints(pointList, '.');

            notdone = true;
            while(notdone)
            {
                BoardContext.DrawBoard(message);

                message = "Welcome " + username + " (" + BitConverter.ToString(hashUsername).Replace("-", "") + ")";

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
