using NeoLux;
using NPC.dApps.NeoDraw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPC.dApp.NeoDraw.ClientApp
{
    public static class NPCHelpers
    {
        public const string NeoDraw_ContractHash = "694ebe0840d1952b09f5435152eebbbc1f8e4b8e";

        public static UserPoint[] GetAllPoints(byte[] encodedUserName)
        {
            List<UserPoint> points = new List<UserPoint>();

            var api = NeoDB.ForTestNet();
            var response = api.TestInvokeScript(NeoDraw_ContractHash, new object[] { "getall", "point", new object[] { "100" } });
            object[] resultsArray = (object[])response.result;
            Console.WriteLine("resultsArray.length: " + resultsArray.Length);
            for (int element = 0; element < resultsArray.Length; element++)
            {
                byte[] bx = (byte[])(((object[])((object[])resultsArray[element])[0])[0]);
                byte[] by = (byte[])(((object[])((object[])resultsArray[element])[0])[1]);
                string sx = Encoding.ASCII.GetString(bx);
                string sy = Encoding.ASCII.GetString(by);
                Console.WriteLine("sx,sy: " + sx + " " + sy);
                int ix = Int32.Parse(sx);
                int iy = Int32.Parse(sy);
                Console.WriteLine("ix,iy: " + ix.ToString() + " " + iy.ToString());

                points.Add(new UserPoint { x = ix, y = iy });
            }

            return points.ToArray<UserPoint>();
        }
    }
}
