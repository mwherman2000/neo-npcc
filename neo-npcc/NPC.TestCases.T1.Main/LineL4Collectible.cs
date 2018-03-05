using NPC.Runtime;
using Neo.SmartContract.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// NPC.TestCases.T1.Main.Line - Level 4 Collectible
///
/// Processed:      2018-03-05 12:42:53 AM by npcc - NEO Class Framework (NPC) 2.0 Compiler v1.0.0.0
/// NPC Project:    https://github.com/mwherman2000/neo-npcc/blob/master/README.md
/// NPC Lead:       Michael Herman (Toronto) (mwherman@parallelspace.net)
/// </summary>

namespace NPC.TestCases.T1.Main
{
    public partial class Line : NeoTrace /* Level 4 Collectible */
    {
        /// <summary>
        /// Collectible methods (NPC Level 4)
        /// </summary>
        /// <param name="e">e</param>
        /// <param name="vau">vau</param>
        /// <param name="index">index</param>
        /// <returns>bool</returns>
        public static bool PutElement(Line e, NeoVersionedAppUser vau, int index)
        {
            if (NeoVersionedAppUser.IsNull(vau)) return false;

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            NeoStorageKey nsk = NeoStorageKey.New(vau, "Line");

            byte[] bkey;
            e._state = NeoEntityModel.EntityState.PUTTED;
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bSTA), e._state.AsBigInteger());
 
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bBKeyP1), e._bKeyP1); // Template: NPCLevel4APutElement_cs.txt
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bBKeyP2), e._bKeyP2); // Template: NPCLevel4APutElement_cs.txt
            LogExt("PutElement(vau,i).Line", e); // Template: NPCLevel4BGetElement_cs.txt
            return true;
        }

        /// <summary>
        /// Get an element of an array of entities from Storage based on a NeoStorageKey (NPC Level 4)
        /// </summary>
        /// <param name="vau">vau</param>
        /// <param name="index">index</param>
        /// <returns>Line</returns>
        public static Line GetElement(NeoVersionedAppUser vau, int index)
        {
            if (NeoVersionedAppUser.IsNull(vau)) return Null();

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            NeoStorageKey nsk = NeoStorageKey.New(vau, "Line");

            Line e;
            byte[] bkey;
            byte[] bsta = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bSTA));
            NeoTrace.Trace("Get(bkey).Line.bsta", bsta.Length, bsta);
            if (bsta.Length == 0)
            {
                e = Line.Missing();
            }
            else // not MISSING
            {
                /*EXT*/
                byte[] bext = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bEXT));
                int ista = (int)bsta.AsBigInteger();
                NeoEntityModel.EntityState sta = (NeoEntityModel.EntityState)ista;
                if (sta == NeoEntityModel.EntityState.TOMBSTONED)
                {
                    e = Line.Tombstone();
                }
                else // not MISSING && not TOMBSTONED
                {
                    e = new Line();
                    byte[] BKeyP1 = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bBKeyP1)); // Template: NPCLevel4CGetElement_cs.txt
                    byte[] BKeyP2 = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bBKeyP2)); // Template: NPCLevel4CGetElement_cs.txt
                    e._bKeyP1 = BKeyP1; e._bKeyP2 = BKeyP2;  // NPCLevel4DBuryElement_cs.txt
                    e._state = sta;
                    e._state = NeoEntityModel.EntityState.GETTED; /* OVERRIDE */
                }
            }
            LogExt("Get(bkey).Line.e", e);
            return e;
        }

        /// <summary>
        /// Bury an element of an array of entities in Storage based on a NeoStorageKey (NPC Level 4)
        /// </summary>
        /// <param name="vau">vau</param>
        /// <param name="index">index</param>
        /// <returns>Line</returns>
        public static Line BuryElement(NeoVersionedAppUser vau, int index)
        {
            if (NeoVersionedAppUser.IsNull(vau)) // TODO - create NeoEntityModel.EntityState.BADKEY?
            {
                return Line.Null();
            }

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            NeoStorageKey nsk = NeoStorageKey.New(vau, "Line");

            byte[] bkey;
            Line e;
            /*STA*/
            byte[] bsta = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bSTA));
            NeoTrace.Trace("Bury(vau,index).Line.bsta", bsta.Length, bsta);
            if (bsta.Length == 0)
            {
                e = Line.Missing();
            }
            else // not MISSING - bury it
            {
                e = Line.Tombstone(); // TODO - should Bury() preserve the exist field values or re-initialize them? Preserve is cheaper but not as private
                Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bSTA), e._state.AsBigInteger());

                Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bBKeyP1), e._bKeyP1); // NPCLevel4EBuryElement_cs.txt
                Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, bkey = NeoStorageKey.StorageKey(nsk, index, _bBKeyP2), e._bKeyP2); // NPCLevel4EBuryElement_cs.txt
            } // Template: NPCLevel4Part2_cs.txt
            LogExt("Bury(vau,i).Line", e);
            return e;
        }
    }
}