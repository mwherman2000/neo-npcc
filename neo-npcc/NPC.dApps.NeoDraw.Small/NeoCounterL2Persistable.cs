using NPC.Runtime;
using Neo.SmartContract.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// NPC.dApps.NeoDraw.Main.NeoCounter - Level 2 Persistable
///
/// Processed:       2018-03-06 10:27:25 PM by npcc - NEO Class Framework (NPC) 2.0 Compiler v1.0.0.0
/// NPC Project:     https://github.com/mwherman2000/neo-npcc/blob/master/README.md
/// NPC Lead:        Michael Herman (Toronto) (mwherman@parallelspace.net)
/// </summary>

namespace NPC.dApps.NeoDraw.Main
{
    public partial class NeoCounter : NeoTrace /* Level 2 Persistable */
    {
        // Class name and property names
        private const string _className = "NeoCounter";

        private const string _sCurrentNumber = "CurrentNumber"; // Template: NPCLevel2AFieldConsts_cs.txt
        private static readonly byte[] _bCurrentNumber = Helper.AsByteArray(_sCurrentNumber);
        private const string _sSTA = "_STA"; // Template: NPCLevel2BMissing_cs.txt
        private static readonly byte[] _bSTA = Helper.AsByteArray(_sSTA);

        private const string _sEXT = "_EXT";
        private static readonly byte[] _bEXT = Helper.AsByteArray(_sEXT);
        
        // Internal fields
        private const string _classKeyTag = "/#" + _className + ".";
        private static readonly byte[] _bclassKeyTag = Helper.AsByteArray(_classKeyTag);
 
        // Persistable methods
        public static bool IsMissing(NeoCounter e)
        {
            return (e._state == NeoEntityModel.EntityState.MISSING);
        }

        public static NeoCounter Missing()
        {
            NeoCounter e = new NeoCounter();
            e._currentNumber = 0; 
            e._state = NeoEntityModel.EntityState.MISSING;
            LogExt("Missing().NeoCounter", e);
            return e;
        }

        public static bool Put(NeoCounter e, byte[] key)
        {
            if (key.Length == 0) return false;

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            byte[] _bkeyTag = Helper.Concat(key, _bclassKeyTag);

            e._state = NeoEntityModel.EntityState.PUTTED;
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, Helper.Concat(_bkeyTag, _bSTA), e._state.AsBigInteger());

            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, Helper.Concat(_bkeyTag, _bCurrentNumber), e._currentNumber); // Template: NPCLevel2CPut_cs.txt
            LogExt("Put(bkey).NeoCounter", e); // Template: NPCLevel2DPut_cs.txt
            return true;
        }

        public static bool Put(NeoCounter e, string key)
        {
            if (key.Length == 0) return false;
            LogExt("Put(skey).NeoCounter", e);

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            string _skeyTag = key + _classKeyTag;
            Trace("Put(skey)._skeyTag", _skeyTag);

            e._state = NeoEntityModel.EntityState.PUTTED;
            BigInteger bis = e._state.AsBigInteger();
            Trace("Put(skey).bis", bis);
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, _skeyTag + _sSTA, bis);
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, _skeyTag + _sCurrentNumber, e._currentNumber); // Template: NPCLevel2EPut_cs.txt
            LogExt("Put(skey).NeoCounter", e); // Template: NPCLevel2FGet_cs.txt
            return true;
        }

        public static NeoCounter Get(byte[] key)
        {
            if (key.Length == 0) return Null();

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            byte[] _bkeyTag = Helper.Concat(key, _bclassKeyTag);

            NeoCounter e;
            byte[] bsta = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, Helper.Concat(_bkeyTag, _bSTA));
            NeoTrace.Trace("Get(bkey).bsta", bsta.Length, bsta);
            if (bsta.Length == 0)
            {
                e = NeoCounter.Missing();
            }
            else // not MISSING
            {
                byte[] bext = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, Helper.Concat(_bkeyTag, _bEXT));
                int ista = (int)bsta.AsBigInteger();
                NeoEntityModel.EntityState sta = (NeoEntityModel.EntityState)ista;
                e = new NeoCounter();

                BigInteger CurrentNumber = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, Helper.Concat(_bkeyTag, _bCurrentNumber)).AsBigInteger(); //NPCLevel2GGet_cs.txt
                e._currentNumber = CurrentNumber;  // Template: NPCLevel2HGet_cs.txt
                e._state = sta;
                e._state = NeoEntityModel.EntityState.GETTED; /* OVERRIDE */
            }
            LogExt("Get(bkey).NeoCounter", e);
            return e;
        }

        public static NeoCounter Get(string key)
        {
            if (key.Length == 0) return Null();

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            string _skeyTag = key + _classKeyTag;

            NeoCounter e;
            byte[] bsta = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, _skeyTag + _sSTA);
            NeoTrace.Trace("Get(skey).NeoCounter.bsta", bsta.Length, bsta);
            if (bsta.Length == 0)
            {
                e = NeoCounter.Missing();
            }
            else // not MISSING
            {
                byte[] bext = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, _skeyTag + _sEXT);
                int ista = (int)bsta.AsBigInteger();
                NeoEntityModel.EntityState sta = (NeoEntityModel.EntityState)ista;
                e = new NeoCounter();

                BigInteger CurrentNumber = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, _skeyTag + _sCurrentNumber).AsBigInteger(); //NPCLevel2IGet_cs.txt
                NeoTrace.Trace("Get(skey).e._currentNumber", e._currentNumber); // Template: NPCLevel2Part2_cs.txt
                e._currentNumber = CurrentNumber; 
                e._state = sta;
                e._state = NeoEntityModel.EntityState.GETTED; /* OVERRIDE */
            }
            LogExt("Get(skey).NeoCounter", e);
            return e;
        }
    }
}