﻿using NPC.dApps.NeoDraw;
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
        public const string ProgramName = "NeoDraw - the NEO Blockchain Multi-User Whiteboard dApp";
        public const string helpMessage = "Enter add, delete, get, exit, or help";

        public const int MAXPOINTS = 20;
        public const int NROWS = 40;
        public const int NCOLS = 50;
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
            Console.Write("0,0 +"); for (int icol = 0; icol < Constants.NCOLS*2 + 1; icol++) Console.Write("-"); Console.WriteLine("+");
            for (int irow = 0; irow < Constants.NROWS; irow++)
            {
                Console.Write("    "); Console.Write("|");
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
                        case '!':
                            {
                                Console.Beep();
                                ConsoleColor saved = Console.ForegroundColor;
                                Console.ForegroundColor = ConsoleColor.Red;
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
            Console.Write("    +"); for (int icol = 0; icol < Constants.NCOLS*2 + 1; icol++) Console.Write("-");
            Console.WriteLine("+ {0},{1}", Constants.NROWS, Constants.NCOLS);
            Console.WriteLine();
        }

        public static void DrawVectors(UserContext uc, char ch)
        {
            bool firstVisiblePointDrawn = false;
            for (int pindex = 1; pindex < uc.NPoints; pindex++) // for every pair of points....
            {
                List<UserPoint> vectorPoints = Bresenham.GetLinePoints(uc.Points[pindex-1], uc.Points[pindex]); // draw vector
                foreach (UserPoint p in vectorPoints)
                {
                    if (p.x < 0 || p.x >= Constants.NCOLS) break;
                    if (p.y < 0 || p.y >= Constants.NROWS) break;

                    if (Char.IsWhiteSpace(Rows[(int)p.y].Columns[(int)p.x])
                        /*|| Char.IsLetterOrDigit(Rows[(int)p.y].Columns[(int)p.x])*/
                        /*|| Rows[(int)p.y].Columns[(int)p.x] != ch*/)
                    {
                        // Do nothing - no BAM!
                    }
                    else 
                    {
                        string name = Constants.BAM;
                        int xleft = (int)p.x + 1;
                        int xright = (int)p.x + name.Length;
                        if (xright >= (Constants.NCOLS - 1)) xleft = (int)p.x - name.Length;
                        for (int xoffset = 0; xoffset < name.Length; xoffset++)
                        {
                            Rows[(int)p.y].Columns[xleft + xoffset] = Constants.BAM.ToCharArray()[xoffset];
                        }
                    }
                    Rows[(int)p.y].Columns[(int)p.x] = ch;

                    if (!firstVisiblePointDrawn) // Draw name
                    {
                        string name = uc.VectorStartNames[0];
                        int xleft = (int)p.x + 1;
                        int xright = (int)p.x + name.Length;
                        if (xright >= (Constants.NCOLS - 1)) xleft = (int)p.x - name.Length;
                        for (int xoffset = 0; xoffset < name.Length; xoffset++)
                        {
                            if ((int)p.y - 1 < 0)
                            {
                                Rows[(int)p.y + 1].Columns[xleft + xoffset] = name.ToCharArray()[xoffset];
                            }
                            else
                            {
                                Rows[(int)p.y - 1].Columns[xleft + xoffset] = name.ToCharArray()[xoffset];
                            }
                        }
                        firstVisiblePointDrawn = true;
                    }
                }
            }
        }
    }

    class UserContext
    {
        public string Username;
        public byte[] EncodedUsername;
        public byte[] EncodedPassword;
        public int NPoints = 0;
        public UserPoint[] Points = new UserPoint[Constants.MAXPOINTS];
        public string[] VectorStartNames = new string[Constants.MAXPOINTS];
        
        public UserContext(string username, byte[] encodedUsername, byte[] encodedPassword)
        {
            this.Username = username;
            this.EncodedUsername = encodedUsername;
            this.EncodedPassword = encodedPassword;
        }

        public UserContext(string username, string encodedUsername, string encodedPassword)
        {
            this.Username = username;
            this.EncodedUsername = Encoding.ASCII.GetBytes(encodedUsername);
            this.EncodedPassword = Encoding.ASCII.GetBytes(encodedPassword);
        }

        public void AddPoint(UserPoint up)
        {
            Points[NPoints] = up;
            VectorStartNames[NPoints] = "";
            NPoints++;
        }

        public void AddPoint(UserPoint up, string name)
        {
            Points[NPoints] = up;
            VectorStartNames[NPoints] = name;
            NPoints++;
        }
    }

    class UsersContext
    {
        public Dictionary<string, UserContext> uc = new Dictionary<string, UserContext>(); // indexed by username (unencoded)
    }

    class Program
    {
        static void Main(string[] args)
        {
            bool notdone = true;
            string username = "";
            string password = "";
            string command = "";
            string secretPhrase = "";
            string message = "Welcome to " + Constants.ProgramName;

            UsersContext uctx = new UsersContext();

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
            UserContext ucHedge = new UserContext("Hedge", "Hedge", "Hedge");
            UserPoint[] pointsHedge = new UserPoint[] { new UserPoint { x = 20, y = 2 }, new UserPoint { x = 40, y = 100 } };
            bool firstPoint = true;
            foreach (UserPoint up in pointsHedge)
            {
                if (firstPoint)
                {
                    ucHedge.AddPoint(up, "Hedge");
                    firstPoint = false;
                }
                else
                {
                    ucHedge.AddPoint(up);
                }
            }
            uctx.uc.Add("Hedge", ucHedge);

            UserContext ucWall = new UserContext("Wall", "Wall", "Wall");
            UserPoint[] pointsWall = new UserPoint[] { new UserPoint { x = 2, y = 3 }, new UserPoint { x = 30, y = 5 } };
            firstPoint = true;
            foreach (UserPoint up in pointsWall)
            {
                if (firstPoint)
                {
                    ucWall.AddPoint(up, "Wall");
                    firstPoint = false;
                }
                else
                {
                    ucWall.AddPoint(up);
                }
            }
            uctx.uc.Add("Wall", ucWall);

            UserContext uc100 = new UserContext("100", "100", "100");
            UserPoint[] points100 = NPCHelpers.GetAllPoints(uc100.EncodedUsername);
            firstPoint = true;
            foreach (UserPoint up in points100)
            {
                if (firstPoint)
                {
                    uc100.AddPoint(up, "100");
                    firstPoint = false;
                }
                else
                {
                    uc100.AddPoint(up);
                }
            }
            uctx.uc.Add("100", uc100);

            BoardContext.Initialize();
            BoardContext.DrawVectors(ucHedge, '*');
            BoardContext.DrawVectors(ucWall, '.');
            BoardContext.DrawVectors(uc100, '+');

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
                    message = Constants.helpMessage;
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
                            message = Constants.helpMessage;
                            break;
                        }
                    default:
                        {
                            message = Constants.helpMessage;
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
            if (Trace.Splash) Console.WriteLine(" " + Constants.ProgramName + " v" + Assembly.GetEntryAssembly().GetName().Version.ToString());
            if (Trace.Splash) { for (int icol = 0; icol < Constants.NCOLS * 2 + 6; icol++) Console.Write("*"); Console.WriteLine(); }
            if (!String.IsNullOrEmpty(message))
            {
                Console.Write(" ");  Console.WriteLine(message);
                Console.WriteLine();
            }
        }
    }
}
