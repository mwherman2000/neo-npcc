using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPC.dApp.NeoDraw.ClientApp
{
    public class Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    public static class Bresenham
    {
        // Reference: https://stackoverflow.com/questions/11678693/all-cases-covered-bresenhams-line-algorithm
        public static List<Point> GetLinePoints(Point p1, Point p2)
        {
            List<Point> pointList = new List<Point>();

            int dx = p2.X - p1.X;
            int dy = p2.Y - p1.Y;

            int swaps = 0;
            if (dy > dx)
            {
                Swap(ref dx, ref dy);
                swaps = 1;
            }

            int a = Math.Abs(dy);
            int b = -Math.Abs(dx);

            double d = 2 * a + b;
            int x = p1.X;
            int y = p1.Y;
            //color_track = Color.Blue;
            //Check_Pixel(ref area, new Point(x, y));
            pointList.Clear();
            pointList.Add(new Point(x, y));

            int s = 1;
            int q = 1;
            if (p1.X > p2.X) q = -1;
            if (p1.Y > p2.Y) s = -1;

            for (int k = 0; k < dx; k++)
            {
                if (d >= 0)
                {
                    d = 2 * (a + b) + d;
                    y = y + s;
                    x = x + q;
                }
                else
                {
                    if (swaps == 1) y = y + s;
                    else x = x + q;
                    d = 2 * a + d;
                }
                //Check_Pixel(ref area, new Point(x, y));
                pointList.Add(new Point(x, y));
            }

            return pointList;
        }

        private static void Swap(ref int x, ref int y)
        {
            int temp = x;
            x = y;
            y = temp;
        }
    }
}
