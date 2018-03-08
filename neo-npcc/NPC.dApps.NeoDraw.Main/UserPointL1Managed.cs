using NPC.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// NPC.dApps.NeoDraw.Main.UserPoint - Level 1 Managed
///
/// Processed:       2018-03-07 8:34:28 PM by npcc - NEO Class Framework (NPC) 2.0 Compiler v1.0.0.0
/// NPC Project:     https://github.com/mwherman2000/neo-npcc/blob/master/README.md
/// NPC Lead:        Michael Herman (Toronto) (mwherman@parallelspace.net)
/// </summary>

namespace NPC.dApps.NeoDraw.Main
{
    public partial class UserPoint : NeoTraceRuntime /* Level 1 Managed */
    {
        private NeoEntityModel.EntityState _state;

        // Hidden constructor
        private UserPoint()
        {
        }

        // Accessors

        public static void SetX(UserPoint e, BigInteger value) // Template: NPCLevel1SetXGetX_cs.txt
                               { e._x = value; e._state = NeoEntityModel.EntityState.SET; }
        public static BigInteger GetX(UserPoint e) { return e._x; }
        public static void SetY(UserPoint e, BigInteger value) // Template: NPCLevel1SetXGetX_cs.txt
                               { e._y = value; e._state = NeoEntityModel.EntityState.SET; }
        public static BigInteger GetY(UserPoint e) { return e._y; }
        public static void Set(UserPoint e, BigInteger X, BigInteger Y) // Template: NPCLevel1Set_cs.txt
                                { e._x = X; e._y = Y;  e._state = NeoEntityModel.EntityState.SET; }        
        // Factory methods // Template: NPCLevel1Part2_cs.txt
        private static UserPoint _Initialize(UserPoint e)
        {
            e._x = 0; e._y = 0; 
            e._state = NeoEntityModel.EntityState.NULL;
            if (NeoTrace.RUNTIME) LogExt("_Initialize(e).UserPoint", e);
            return e;
        }
        public static UserPoint New()
        {
            UserPoint e = new UserPoint();
            _Initialize(e);
            if (NeoTrace.RUNTIME) LogExt("New().UserPoint", e);
            return e;
        }
        public static UserPoint New(BigInteger X, BigInteger Y)
        {
            UserPoint e = new UserPoint();
            e._x = X; e._y = Y; 
            e._state = NeoEntityModel.EntityState.INIT;
            if (NeoTrace.RUNTIME) LogExt("New(.,.).UserPoint", e);
            return e;
        }
        public static UserPoint Null()
        {
            UserPoint e = new UserPoint();
            _Initialize(e);
            if (NeoTrace.RUNTIME) LogExt("Null().UserPoint", e);
            return e;
        }

        // EntityState wrapper methods
        public static bool IsNull(UserPoint e)
        {
            return (e._state == NeoEntityModel.EntityState.NULL);
        }

        // Log/trace methods
        public static void Log(string label, UserPoint e)
        {
            TraceRuntime(label, e._x, e._y);
        }
        public static void LogExt(string label, UserPoint e)
        {
            TraceRuntime(label, e._x, e._y, e._state);
        }
    }
}