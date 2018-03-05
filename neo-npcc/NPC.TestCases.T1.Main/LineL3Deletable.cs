using NPC.Runtime;
using Neo.SmartContract.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// NPC.TestCases.T1.Main.Line - Level 3 Deletable
///
/// Processed:      2018-03-05 12:42:53 AM by npcc - NEO Class Framework (NPC) 2.0 Compiler v1.0.0.0
/// NPC Project:    https://github.com/mwherman2000/neo-npcc/blob/master/README.md
/// NPC Lead:       Michael Herman (Toronto) (mwherman@parallelspace.net)
/// </summary>

namespace NPC.TestCases.T1.Main
{
    public partial class Line : NeoTrace /* Level 3 Deletable */
    {
        // Deletable methods
        public static bool IsBuried(Line e)
        {
            return (e._state == NeoEntityModel.EntityState.TOMBSTONED);
        }

        public static Line Tombstone()
        {
            Line e = new Line();
            e._bKeyP1 = NeoEntityModel.NullByteArray; e._bKeyP2 = NeoEntityModel.NullByteArray; 
            e._state = NeoEntityModel.EntityState.TOMBSTONED;
            LogExt("Tombstone().Line", e);
            return e;
        }

        public static Line Bury(byte[] key)
        {
            if (key.Length == 0) return Null();

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            byte[] _bkeyTag = Helper.Concat(key, _bclassKeyTag);

            Line e;
            byte[] bsta = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, Helper.Concat(_bkeyTag, _bSTA));
            NeoTrace.Trace("Bury(bkey).bsta", bsta.Length, bsta);
            if (bsta.Length == 0)
            {
                e = Line.Missing();
            }
            else // not MISSING - bury it
            {
                e = Line.Tombstone(); // but don't overwrite existing field values - just tombstone it
                Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, Helper.Concat(_bkeyTag, _bSTA), e._state.AsBigInteger());

                //Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, Helper.Concat(_bkeyTag, _bBKeyP1), e._bKeyP1); // Template: NPCLevel3ABury_cs.txt
                //Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, Helper.Concat(_bkeyTag, _bBKeyP2), e._bKeyP2); // Template: NPCLevel3ABury_cs.txt
            } // Template: NPCLevel3BBury_cs.txt
            LogExt("Bury(bkey).Line", e); 
            return e; // return Entity e to signal if key is Missing or bad key
        }

        public static Line Bury(string key)
        {
            if (key.Length == 0) return Null(); 

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            string _skeyTag = key + _classKeyTag;

            Line e;
            byte[] bsta = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, _skeyTag + _sSTA);
            NeoTrace.Trace("Bury(skey).Line.bsta", bsta.Length, bsta);
            if (bsta.Length == 0)
            {
                e = Line.Missing();
            }
            else // not MISSING - bury it
            {
                e = Line.Tombstone(); // but don't overwrite existing field values - just tombstone it
                Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, _skeyTag + _sSTA, e._state.AsBigInteger());

                //Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, _skeyTag + _sBKeyP1, e._bKeyP1); // Template: NPCLevel3CBury_cs.txt
                //Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, _skeyTag + _sBKeyP2, e._bKeyP2); // Template: NPCLevel3CBury_cs.txt
            } // Template: NPCLevel3Part2_cs.txt
            LogExt("Bury(skey).Line", e);
            return e; // return Entity e to signal if key is Missing or bad key
        }
    }
}