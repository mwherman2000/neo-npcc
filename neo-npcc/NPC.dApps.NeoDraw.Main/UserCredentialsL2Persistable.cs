using NPC.Runtime;
using Neo.SmartContract.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// NPC.dApps.NeoDraw.Main.UserCredentials - Level 2 Persistable
///
/// Processed:       2018-03-04 8:25:07 PM by npcc - NEO Class Framework (NPC) 2.0 Compiler v1.0.0.0
/// NPC Project:     https://github.com/mwherman2000/neo-npcc/blob/master/README.md
/// NPC Lead:        Michael Herman (Toronto) (mwherman@parallelspace.net)
/// </summary>

namespace NPC.dApps.NeoDraw.Main
{
    public partial class UserCredentials : NeoTrace /* Level 2 Persistable */
    {
        // Class name and property names
        private const string _className = "UserCredentials";

        private const string _sEncodedUsername = "EncodedUsername"; // Template: NPCLevel2AFieldConsts_cs.txt
        private static readonly byte[] _bEncodedUsername = Helper.AsByteArray(_sEncodedUsername);
        private const string _sEncodedPassword = "EncodedPassword"; // Template: NPCLevel2AFieldConsts_cs.txt
        private static readonly byte[] _bEncodedPassword = Helper.AsByteArray(_sEncodedPassword);
        private const string _sSTA = "_STA"; // Template: NPCLevel2BMissing_cs.txt
        private static readonly byte[] _bSTA = Helper.AsByteArray(_sSTA);

        private const string _sEXT = "_EXT";
        private static readonly byte[] _bEXT = Helper.AsByteArray(_sEXT);
        
        // Internal fields
        private const string _classKeyTag = "/#" + _className + ".";
        private static readonly byte[] _bclassKeyTag = Helper.AsByteArray(_classKeyTag);
 
        // Persistable methods
        public static bool IsMissing(UserCredentials e)
        {
            return (e._state == NeoEntityModel.EntityState.MISSING);
        }

        public static UserCredentials Missing()
        {
            UserCredentials e = new UserCredentials();
            e._encodedUsername = NeoEntityModel.NullByteArray; e._encodedPassword = NeoEntityModel.NullByteArray; 
            e._state = NeoEntityModel.EntityState.MISSING;
            LogExt("Missing().UserCredentials", e);
            return e;
        }

        public static bool Put(UserCredentials e, byte[] key)
        {
            if (key.Length == 0) return false;

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            byte[] _bkeyTag = Helper.Concat(key, _bclassKeyTag);

            e._state = NeoEntityModel.EntityState.PUTTED;
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, Helper.Concat(_bkeyTag, _bSTA), e._state.AsBigInteger());

            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, Helper.Concat(_bkeyTag, _bEncodedUsername), e._encodedUsername); // Template: NPCLevel2CPut_cs.txt
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, Helper.Concat(_bkeyTag, _bEncodedPassword), e._encodedPassword); // Template: NPCLevel2CPut_cs.txt
            LogExt("Put(bkey).UserCredentials", e); // Template: NPCLevel2DPut_cs.txt
            return true;
        }

        public static bool Put(UserCredentials e, string key)
        {
            if (key.Length == 0) return false;
            LogExt("Put(skey).UserCredentials", e);

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            string _skeyTag = key + _classKeyTag;
            Trace("Put(skey)._skeyTag", _skeyTag);

            e._state = NeoEntityModel.EntityState.PUTTED;
            BigInteger bis = e._state.AsBigInteger();
            Trace("Put(skey).bis", bis);
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, _skeyTag + _sSTA, bis);
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, _skeyTag + _sEncodedUsername, e._encodedUsername); // Template: NPCLevel2EPut_cs.txt
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, _skeyTag + _sEncodedPassword, e._encodedPassword); // Template: NPCLevel2EPut_cs.txt
            LogExt("Put(skey).UserCredentials", e); // Template: NPCLevel2FGet_cs.txt
            return true;
        }

        public static UserCredentials Get(byte[] key)
        {
            if (key.Length == 0) return Null();

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            byte[] _bkeyTag = Helper.Concat(key, _bclassKeyTag);

            UserCredentials e;
            byte[] bsta = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, Helper.Concat(_bkeyTag, _bSTA));
            NeoTrace.Trace("Get(bkey).bsta", bsta.Length, bsta);
            if (bsta.Length == 0)
            {
                e = UserCredentials.Missing();
            }
            else // not MISSING
            {
                byte[] bext = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, Helper.Concat(_bkeyTag, _bEXT));
                int ista = (int)bsta.AsBigInteger();
                NeoEntityModel.EntityState sta = (NeoEntityModel.EntityState)ista;
                e = new UserCredentials();

                byte[] EncodedUsername = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, Helper.Concat(_bkeyTag, _bEncodedUsername)); //NPCLevel2GGet_cs.txt
                byte[] EncodedPassword = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, Helper.Concat(_bkeyTag, _bEncodedPassword)); //NPCLevel2GGet_cs.txt
                e._encodedUsername = EncodedUsername; e._encodedPassword = EncodedPassword;  // Template: NPCLevel2HGet_cs.txt
                e._state = sta;
                e._state = NeoEntityModel.EntityState.GETTED; /* OVERRIDE */
            }
            LogExt("Get(bkey).UserCredentials", e);
            return e;
        }

        public static UserCredentials Get(string key)
        {
            if (key.Length == 0) return Null();

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            string _skeyTag = key + _classKeyTag;

            UserCredentials e;
            byte[] bsta = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, _skeyTag + _sSTA);
            NeoTrace.Trace("Get(skey).UserCredentials.bsta", bsta.Length, bsta);
            if (bsta.Length == 0)
            {
                e = UserCredentials.Missing();
            }
            else // not MISSING
            {
                byte[] bext = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, _skeyTag + _sEXT);
                int ista = (int)bsta.AsBigInteger();
                NeoEntityModel.EntityState sta = (NeoEntityModel.EntityState)ista;
                e = new UserCredentials();

                byte[] EncodedUsername = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, _skeyTag + _sEncodedUsername); //NPCLevel2IGet_cs.txt
                byte[] EncodedPassword = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, _skeyTag + _sEncodedPassword); //NPCLevel2IGet_cs.txt
                NeoTrace.Trace("Get(skey).e._encodedUsername, e._encodedPassword", e._encodedUsername, e._encodedPassword); // Template: NPCLevel2Part2_cs.txt
                e._encodedUsername = EncodedUsername; e._encodedPassword = EncodedPassword; 
                e._state = sta;
                e._state = NeoEntityModel.EntityState.GETTED; /* OVERRIDE */
            }
            LogExt("Get(skey).UserCredentials", e);
            return e;
        }
    }
}