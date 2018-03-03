using NPC.Runtime;
using Neo.SmartContract.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// NPC.dApps.NeoDraw.Main.NeoCounter - Level 3 Deletable
///
/// Generated:      2018-03-03 9:06:59 AM by npcc - NEO Class Framework (NPC) 2.0 Compiler v1.0.0.0
/// NPC Project:    https://github.com/mwherman2000/neo-npcc/blob/master/README.md
/// NPC Lead:       Michael Herman (Toronto) (mwherman@parallelspace.net)
/// </summary>

namespace NPC.dApps.NeoDraw.Main
{
    public partial class NeoCounter : NeoTrace /* Level 3 Deletable */
    {
        // Deletable methods
        public static bool IsBuried(NeoCounter e)
        {
            return (e._state == NeoEntityModel.EntityState.TOMBSTONED);
        }

        public static NeoCounter Tombstone()
        {
            NeoCounter e = new NeoCounter();
            e._nextIndex = 0; 
            e._state = NeoEntityModel.EntityState.TOMBSTONED;
            LogExt("Tombstone().NeoCounter", e);
            return e;
        }

        public static NeoCounter Bury(byte[] key)
        {
            if (key.Length == 0) return Null();

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            byte[] _bkeyTag = Helper.Concat(key, _bclassKeyTag);

            NeoCounter e;
            byte[] bsta = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, Helper.Concat(_bkeyTag, _bSTA));
            NeoTrace.Trace("Bury(bkey).bsta", bsta.Length, bsta);
            if (bsta.Length == 0)
            {
                e = NeoCounter.Missing();
            }
            else // not MISSING - bury it
            {
                e = NeoCounter.Tombstone(); // but don't overwrite existing field values - just tombstone it
                Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, Helper.Concat(_bkeyTag, _bSTA), e._state.AsBigInteger());

                //Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, Helper.Concat(_bkeyTag, _bNextIndex), e._nextIndex); // Template: NPCLevel3ABury_cs.txt
            } // Template: NPCLevel3BBury_cs.txt
            LogExt("Bury(bkey).NeoCounter", e); 
            return e; // return Entity e to signal if key is Missing or bad key
        }

        public static NeoCounter Bury(string key)
        {
            if (key.Length == 0) return Null(); 

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            string _skeyTag = key + _classKeyTag;

            NeoCounter e;
            byte[] bsta = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, _skeyTag + _sSTA);
            NeoTrace.Trace("Bury(skey).NeoCounter.bsta", bsta.Length, bsta);
            if (bsta.Length == 0)
            {
                e = NeoCounter.Missing();
            }
            else // not MISSING - bury it
            {
                e = NeoCounter.Tombstone(); // but don't overwrite existing field values - just tombstone it
                Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, _skeyTag + _sSTA, e._state.AsBigInteger());

                //Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, _skeyTag + _sNextIndex, e._nextIndex); // Template: NPCLevel3CBury_cs.txt
            } // Template: NPCLevel3Part2_cs.txt
            LogExt("Bury(skey).NeoCounter", e);
            return e; // return Entity e to signal if key is Missing or bad key
        }
    }
}