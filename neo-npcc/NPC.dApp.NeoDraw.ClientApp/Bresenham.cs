using NPC.dApps.NeoDraw;
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
        public static List<UserPoint> GetLinePoints(UserPoint p1, UserPoint p2)
        {
            List<UserPoint> pointList = new List<UserPoint>();

            int _dx = (int)(p2.x) - (int)p1.x;
            int _dy = (int)(p2.y) - (int)p1.y;

            int swaps = 0;
            if (_dy > _dx)
            {
                Swap(ref _dx, ref _dy);
                swaps = 1;
            }

            int a = Math.Abs(_dy);
            int b = -Math.Abs(_dx);

            double d = 2 * a + b;
            int _x = (int)p1.x;
            int _y = (int)p1.y;
            //color_track = Color.Blue;
            //Check_Pixel(ref area, new Point(x, y));
            pointList.Clear();
            pointList.Add(new UserPoint { x = _x, y = _y });

            int s = 1;
            int q = 1;
            if (p1.x > p2.x) q = -1;
            if (p1.y > p2.y) s = -1;

            for (int k = 0; k < _dx; k++)
            {
                if (d >= 0)
                {
                    d = 2 * (a + b) + d;
                    _y = _y + s;
                    _x = _x + q;
                }
                else
                {
                    if (swaps == 1) _y = _y + s;
                    else _x = _x + q;
                    d = 2 * a + d;
                }
                //Check_Pixel(ref area, new Point(x, y));
                pointList.Add(new UserPoint { x = _x, y = _y });
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
