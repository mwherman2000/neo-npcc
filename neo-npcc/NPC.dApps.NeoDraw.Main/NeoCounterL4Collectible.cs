using NPC.Runtime;
using Neo.SmartContract.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// NPC.dApps.NeoDraw.Main.NeoCounter - Level 4 Collectible
///
/// Generated:      2018-03-03 9:06:59 AM by npcc - NEO Class Framework (NPC) 2.0 Compiler v1.0.0.0
/// NPC Project:    https://github.com/mwherman2000/neo-npcc/blob/master/README.md
/// NPC Lead:       Michael Herman (Toronto) (mwherman@parallelspace.net)
/// </summary>

namespace NPC.dApps.NeoDraw.Main
{
    public partial class NeoCounter : NeoTrace /* Level 4 Collectible */
    {
        /// <summary>
        /// Collectible methods (NPC Level 4)
        /// </summary>
        /// <param name="e">e</param>
        /// <param name="vau">vau</param>
        /// <param name="index">index</param>
        /// <returns>bool</returns>
        public static bool PutElement(NeoCounter e, NeoVersionedAppUser vau, int index)
        {
            if (NeoVersionedAppUser.IsNull(vau)) return false;

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            NeoStorageKey nsk = NeoStorageKey.New(vau, "NeoCounter");

            byte[] bkey;
            e._state = NeoEntityModel.EntityState.PUTTED;
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bSTA), e._state.AsBigInteger());
 
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bNextIndex), e._nextIndex); // Template: NPCLevel4APutElement_cs.txt
            LogExt("PutElement(vau,i).NeoCounter", e); // Template: NPCLevel4BGetElement_cs.txt
            return true;
        }

        /// <summary>
        /// Get an element of an array of entities from Storage based on a NeoStorageKey (NPC Level 4)
        /// </summary>
        /// <param name="vau">vau</param>
        /// <param name="index">index</param>
        /// <returns>NeoCounter</returns>
        public static NeoCounter GetElement(NeoVersionedAppUser vau, int index)
        {
            if (NeoVersionedAppUser.IsNull(vau)) return Null();

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            NeoStorageKey nsk = NeoStorageKey.New(vau, "NeoCounter");

            NeoCounter e;
            byte[] bkey;
            byte[] bsta = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bSTA));
            NeoTrace.Trace("Get(bkey).NeoCounter.bsta", bsta.Length, bsta);
            if (bsta.Length == 0)
            {
                e = NeoCounter.Missing();
            }
            else // not MISSING
            {
                /*EXT*/
                byte[] bext = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bEXT));
                int ista = (int)bsta.AsBigInteger();
                NeoEntityModel.EntityState sta = (NeoEntityModel.EntityState)ista;
                if (sta == NeoEntityModel.EntityState.TOMBSTONED)
                {
                    e = NeoCounter.Tombstone();
                }
                else // not MISSING && not TOMBSTONED
                {
                    e = new NeoCounter();
                    BigInteger NextIndex = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bNextIndex)).AsBigInteger(); // Template: NPCLevel4CGetElement_cs.txt
                    e._nextIndex = NextIndex;  // NPCLevel4DBuryElement_cs.txt
                    e._state = sta;
                    e._state = NeoEntityModel.EntityState.GETTED; /* OVERRIDE */
                }
            }
            LogExt("Get(bkey).NeoCounter.e", e);
            return e;
        }

        /// <summary>
        /// Bury an element of an array of entities in Storage based on a NeoStorageKey (NPC Level 4)
        /// </summary>
        /// <param name="vau">vau</param>
        /// <param name="index">index</param>
        /// <returns>NeoCounter</returns>
        public static NeoCounter BuryElement(NeoVersionedAppUser vau, int index)
        {
            if (NeoVersionedAppUser.IsNull(vau)) // TODO - create NeoEntityModel.EntityState.BADKEY?
            {
                return NeoCounter.Null();
            }

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            NeoStorageKey nsk = NeoStorageKey.New(vau, "NeoCounter");

            byte[] bkey;
            NeoCounter e;
            /*STA*/
            byte[] bsta = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bSTA));
            NeoTrace.Trace("Bury(vau,index).NeoCounter.bsta", bsta.Length, bsta);
            if (bsta.Length == 0)
            {
                e = NeoCounter.Missing();
            }
            else // not MISSING - bury it
            {
                e = NeoCounter.Tombstone(); // TODO - should Bury() preserve the exist field values or re-initialize them? Preserve is cheaper but not as private
                Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bSTA), e._state.AsBigInteger());

                Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bNextIndex), e._nextIndex); // NPCLevel4EBuryElement_cs.txt
            } // Template: NPCLevel4Part2_cs.txt
            LogExt("Bury(vau,i).NeoCounter", e);
            return e;
        }
    }
}