using NPC.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// NPC.TestCases.T1.Main.Point - Level 1 Managed
///
/// Generated:       2018-03-03 12:56:55 AM by npcc - NEO Class Framework (NPC) 2.0 Compiler v1.0.0.0
/// NPC Project:     https://github.com/mwherman2000/neo-npcc/blob/master/README.md
/// NPC Lead:        Michael Herman (Toronto) (mwherman@parallelspace.net)
/// </summary>

namespace NPC.TestCases.T1.Main
{
    public partial class Point : NeoTrace /* Level 1 Managed */
    {
        private NeoEntityModel.EntityState _state;

        // Hidden constructor
        private Point()
        {
        }

        // Accessors

        public static void SetX(Point e, BigInteger value) // Template: NPCLevel1SetXGetX_cs.txt
                               { e._x = value; e._state = NeoEntityModel.EntityState.SET; }
        public static BigInteger GetX(Point e) { return e._x; }
        public static void SetY(Point e, BigInteger value) // Template: NPCLevel1SetXGetX_cs.txt
                               { e._y = value; e._state = NeoEntityModel.EntityState.SET; }
        public static BigInteger GetY(Point e) { return e._y; }
        public static void Set(Point e, BigInteger X, BigInteger Y) // Template: NPCLevel1Set_cs.txt
                                { e._x = X; e._y = Y;  e._state = NeoEntityModel.EntityState.SET; }        
        // Factory methods // Template: NPCLevel1Part2_cs.txt
        private static Point _Initialize(Point e)
        {
            e._x = 0; e._y = 0; 
            e._state = NeoEntityModel.EntityState.NULL;
            LogExt("_Initialize(e).Point", e);
            return e;
        }
        public static Point New()
        {
            Point e = new Point();
            _Initialize(e);
            LogExt("New().Point", e);
            return e;
        }
        public static Point New(BigInteger X, BigInteger Y)
        {
            Point e = new Point();
            e._x = X; e._y = Y; 
            e._state = NeoEntityModel.EntityState.INIT;
            LogExt("New(.,.).Point", e);
            return e;
        }
        public static Point Null()
        {
            Point e = new Point();
            _Initialize(e);
            LogExt("Null().Point", e);
            return e;
        }

        // EntityState wrapper methods
        public static bool IsNull(Point e)
        {
            return (e._state == NeoEntityModel.EntityState.NULL);
        }

        // Log/trace methods
        public static void Log(string label, Point e)
        {
            NeoTrace.Trace(label, e._x, e._y);
        }
        public static void LogExt(string label, Point e)
        {
            NeoTrace.Trace(label, e._x, e._y, e._state);
        }
    }
}