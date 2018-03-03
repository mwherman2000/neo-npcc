using NPC.Runtime;
using Neo.SmartContract.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// NPC.TestCases.T1.Main.Point - Level 2 Persistable
///
/// Generated:       2018-03-03 12:40:42 AM by npcc - NEO Class Framework (NPC) 2.0 Compiler v1.0.0.0
/// NPC Project:     https://github.com/mwherman2000/neo-npcc/blob/master/README.md
/// NPC Lead:        Michael Herman (Toronto) (mwherman@parallelspace.net)
/// </summary>

namespace NPC.TestCases.T1.Main
{
    public partial class Point : NeoTrace /* Level 2 Persistable */
    {
        // Class name and property names
        private const string _className = "Point";

        private const string _sX = "X"; // Template: NPCLevel2AFieldConsts_cs.txt
        private static readonly byte[] _bX = Helper.AsByteArray(_sX);
        private const string _sY = "Y"; // Template: NPCLevel2AFieldConsts_cs.txt
        private static readonly byte[] _bY = Helper.AsByteArray(_sY);
        private const string _sSTA = "_STA"; // Template: NPCLevel2BMissing_cs.txt
        private static readonly byte[] _bSTA = Helper.AsByteArray(_sSTA);

        private const string _sEXT = "_EXT";
        private static readonly byte[] _bEXT = Helper.AsByteArray(_sEXT);
        
        // Internal fields
        private const string _classKeyTag = "/#" + _className + ".";
        private static readonly byte[] _bclassKeyTag = Helper.AsByteArray(_classKeyTag);
 
        // Persistable methods
        public static bool IsMissing(Point e)
        {
            return (e._state == NeoEntityModel.EntityState.MISSING);
        }

        public static Point Missing()
        {
            Point e = new Point();
            e._x = 0; e._y = 0; 
            e._state = NeoEntityModel.EntityState.MISSING;
            LogExt("Missing().Point", e);
            return e;
        }

        public static bool Put(Point e, byte[] key)
        {
            if (key.Length == 0) return false;

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            byte[] _bkeyTag = Helper.Concat(key, _bclassKeyTag);

            e._state = NeoEntityModel.EntityState.PUTTED;
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, Helper.Concat(_bkeyTag, _bSTA), e._state.AsBigInteger());

            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, Helper.Concat(_bkeyTag, _bX), e._x); // Template: NPCLevel2CPut_cs.txt
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, Helper.Concat(_bkeyTag, _bY), e._y); // Template: NPCLevel2CPut_cs.txt
            LogExt("Put(bkey).Point", e); // Template: NPCLevel2DPut_cs.txt
            return true;
        }

        public static bool Put(Point e, string key)
        {
            if (key.Length == 0) return false;
            LogExt("Put(skey).Point", e);

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            string _skeyTag = key + _classKeyTag;
            Trace("Put(skey)._skeyTag", _skeyTag);

            e._state = NeoEntityModel.EntityState.PUTTED;
            BigInteger bis = e._state.AsBigInteger();
            Trace("Put(skey).bis", bis);
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, _skeyTag + _sSTA, bis);
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, _skeyTag + _sX, e._x); // Template: NPCLevel2EPut_cs.txt
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, _skeyTag + _sY, e._y); // Template: NPCLevel2EPut_cs.txt
            LogExt("Put(skey).Point", e); // Template: NPCLevel2FGet_cs.txt
            return true;
        }

        public static Point Get(byte[] key)
        {
            if (key.Length == 0) return Null();

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            byte[] _bkeyTag = Helper.Concat(key, _bclassKeyTag);

            Point e;
            byte[] bsta = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, Helper.Concat(_bkeyTag, _bSTA));
            NeoTrace.Trace("Get(bkey).bsta", bsta.Length, bsta);
            if (bsta.Length == 0)
            {
                e = Point.Missing();
            }
            else // not MISSING
            {
                byte[] bext = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, Helper.Concat(_bkeyTag, _bEXT));
                int ista = (int)bsta.AsBigInteger();
                NeoEntityModel.EntityState sta = (NeoEntityModel.EntityState)ista;
                e = new Point();

                BigInteger X = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, Helper.Concat(_bkeyTag, _bX)).AsBigInteger(); //NPCLevel2GGet_cs.txt
                BigInteger Y = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, Helper.Concat(_bkeyTag, _bY)).AsBigInteger(); //NPCLevel2GGet_cs.txt
                e._x = X; e._y = Y;  // Template: NPCLevel2HGet_cs.txt
                e._state = sta;
                e._state = NeoEntityModel.EntityState.GETTED; /* OVERRIDE */
            }
            LogExt("Get(bkey).Point", e);
            return e;
        }

        public static Point Get(string key)
        {
            if (key.Length == 0) return Null();

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            string _skeyTag = key + _classKeyTag;

            Point e;
            byte[] bsta = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, _skeyTag + _sSTA);
            NeoTrace.Trace("Get(skey).Point.bsta", bsta.Length, bsta);
            if (bsta.Length == 0)
            {
                e = Point.Missing();
            }
            else // not MISSING
            {
                byte[] bext = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, _skeyTag + _sEXT);
                int ista = (int)bsta.AsBigInteger();
                NeoEntityModel.EntityState sta = (NeoEntityModel.EntityState)ista;
                e = new Point();

                BigInteger X = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, _skeyTag + _sX).AsBigInteger(); //NPCLevel2IGet_cs.txt
                BigInteger Y = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, _skeyTag + _sY).AsBigInteger(); //NPCLevel2IGet_cs.txt
                NeoTrace.Trace("Get(skey).e._x, e._y", e._x, e._y); // Template: NPCLevel2Part2_cs.txt
                e._x = X; e._y = Y; 
                e._state = sta;
                e._state = NeoEntityModel.EntityState.GETTED; /* OVERRIDE */
            }
            LogExt("Get(skey).Point", e);
            return e;
        }
    }
}