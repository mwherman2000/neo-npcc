using NPC.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// NPC.dApps.NeoDraw.Main.UserCredentials - Level 1 Managed
///
/// Generated:       2018-03-03 9:06:59 AM by npcc - NEO Class Framework (NPC) 2.0 Compiler v1.0.0.0
/// NPC Project:     https://github.com/mwherman2000/neo-npcc/blob/master/README.md
/// NPC Lead:        Michael Herman (Toronto) (mwherman@parallelspace.net)
/// </summary>

namespace NPC.dApps.NeoDraw.Main
{
    public partial class UserCredentials : NeoTrace /* Level 1 Managed */
    {
        private NeoEntityModel.EntityState _state;

        // Hidden constructor
        private UserCredentials()
        {
        }

        // Accessors

        public static void SetEncodedUsername(UserCredentials e, byte[] value) // Template: NPCLevel1SetXGetX_cs.txt
                               { e._encodedUsername = value; e._state = NeoEntityModel.EntityState.SET; }
        public static byte[] GetEncodedUsername(UserCredentials e) { return e._encodedUsername; }
        public static void SetEncodedPassword(UserCredentials e, byte[] value) // Template: NPCLevel1SetXGetX_cs.txt
                               { e._encodedPassword = value; e._state = NeoEntityModel.EntityState.SET; }
        public static byte[] GetEncodedPassword(UserCredentials e) { return e._encodedPassword; }
        public static void Set(UserCredentials e, byte[] EncodedUsername, byte[] EncodedPassword) // Template: NPCLevel1Set_cs.txt
                                { e._encodedUsername = EncodedUsername; e._encodedPassword = EncodedPassword;  e._state = NeoEntityModel.EntityState.SET; }        
        // Factory methods // Template: NPCLevel1Part2_cs.txt
        private static UserCredentials _Initialize(UserCredentials e)
        {
            e._encodedUsername = NeoEntityModel.NullByteArray; e._encodedPassword = NeoEntityModel.NullByteArray; 
            e._state = NeoEntityModel.EntityState.NULL;
            LogExt("_Initialize(e).UserCredentials", e);
            return e;
        }
        public static UserCredentials New()
        {
            UserCredentials e = new UserCredentials();
            _Initialize(e);
            LogExt("New().UserCredentials", e);
            return e;
        }
        public static UserCredentials New(byte[] EncodedUsername, byte[] EncodedPassword)
        {
            UserCredentials e = new UserCredentials();
            e._encodedUsername = EncodedUsername; e._encodedPassword = EncodedPassword; 
            e._state = NeoEntityModel.EntityState.INIT;
            LogExt("New(.,.).UserCredentials", e);
            return e;
        }
        public static UserCredentials Null()
        {
            UserCredentials e = new UserCredentials();
            _Initialize(e);
            LogExt("Null().UserCredentials", e);
            return e;
        }

        // EntityState wrapper methods
        public static bool IsNull(UserCredentials e)
        {
            return (e._state == NeoEntityModel.EntityState.NULL);
        }

        // Log/trace methods
        public static void Log(string label, UserCredentials e)
        {
            NeoTrace.Trace(label, e._encodedUsername, e._encodedPassword);
        }
        public static void LogExt(string label, UserCredentials e)
        {
            NeoTrace.Trace(label, e._encodedUsername, e._encodedPassword, e._state);
        }
    }
}