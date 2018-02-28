using NPC.Runtime;
using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace PartialClassTest1
{
    public class Contract1 : SmartContract
    {
        public static object Main(string operation, params object[] args)
        {
            string msg = "success";
            NeoTrace.Trace("===============================================");
            NeoTrace.Trace("NPCdApp - NEO Persistable Class (NPC) Framework");
            NeoTrace.Trace("NPCdApp - Version 0.1 Reference Implementation");
            NeoTrace.Trace("----------------------------------------------");
            NeoTrace.Trace("operation", operation, args);
            NeoTrace.Trace("===============================================");

            if (operation == "testall" || operation.Length == 0)
            {
                //msg = test1(args);
                msg = test2(args);
                //msg = test3(args);
                //msg = test4(args);
                //msg = test5(args);
                //msg = test6(args);
            }
            //else if (operation == "test1")
            //{
            //    msg = test1(args);
            //}
            else if (operation == "test2")
            {
                msg = test2(args);
            }
            //else if (operation == "test3")
            //{
            //    msg = test3(args);
            //}
            //else if (operation == "test4")
            //{
            //    msg = test4(args);
            //}
            //else if (operation == "test5")
            //{
            //    msg = test5(args);
            //}
            //else if (operation == "test6")
            //{
            //    msg = test6(args);
            //}
            else
            {
                msg = "Unknown operation code";
            }
            NeoTrace.Trace("----------------------------------------");

            return msg;
        }

        public static string test2(object[] args)
        {
            string msg = "success";

            NeoTrace.Trace("Make P0...");
            Point p0 = Point.New();
            Point.Log("p0", p0);
            Point.SetX(p0, 7);
            Point.SetY(p0, 8);
            Point.Log("p0", p0);
            Point.Set(p0, 9, 10);
            Point.Log("p0", p0);

            NeoTrace.Trace("Make P1...");
            Point p1 = Point.New();
            Point.Set(p1, 2, 4);
            Point.Log("p1", p1);

            NeoTrace.Trace("Make P2...");
            Point p2 = Point.New();
            Point.Set(p2, 15, 16);
            Point.Log("p2", p2);

            NeoTrace.Trace("Make line1...");
            Point[] line1 = new[]
            {
                p1, p2
            };
            NeoTrace.Trace("line1", line1, p1, p2); 

            //NeoTrace.Trace("Add 2 points...");
            //Point p3 = Add(line1[0], line1[1]);
            //Point.Log("p3", p3);

            return msg;
        }

        //public static string test3(object[] args)
        //{
        //    string msg = "success";

        //    NeoTrace.Trace("Make P1...");
        //    Point p1 = Point.New();
        //    Point.Set(p1, 2, 4);
        //    Point.Log("p1", p1);

        //    NeoTrace.Trace("Make P2...");
        //    Point p2 = Point.New();
        //    Point.Set(p2, 12, 14);
        //    Point.Log("p2", p2);

        //    NeoTrace.Trace("Make P3...");
        //    Point p3 = Point.New();
        //    Point.Set(p2, 22, 24);
        //    Point.Log("p3", p3);

        //    NeoTrace.Trace("Put P1...");
        //    Point.Put(p1, "p1");
        //    NeoTrace.Trace("Put P2...");
        //    Point.Put(p2, "p2");
        //    NeoTrace.Trace("Put P3...");
        //    Point.Put(p3, "p3");

        //    NeoTrace.Trace("Get P1...");
        //    Point p1get = Point.Get("p1");
        //    Point.Log("p1get", p1get);
        //    NeoTrace.Trace("Get P2...");
        //    Point p2get = Point.Get("p2");
        //    Point.Log("p2get", p2get);
        //    NeoTrace.Trace("Get P3...");
        //    Point p3get = Point.Get("p3");
        //    Point.Log("p3get", p3get);

        //    return msg;
        //}
    }
}
