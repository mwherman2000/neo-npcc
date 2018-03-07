using NPC.Runtime;
using Neo.SmartContract.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// NPC.dApps.NeoDraw.Main.UserPoint - Level 4 Collectible (Extended)
///
/// Processed:      2018-03-06 10:27:26 PM by npcc - NEO Class Framework (NPC) 2.0 Compiler v1.0.0.0
/// NPC Project:    https://github.com/mwherman2000/neo-npcc/blob/master/README.md
/// NPC Lead:       Michael Herman (Toronto) (mwherman@parallelspace.net)
/// </summary>

namespace NPC.dApps.NeoDraw.Main
{
    public partial class UserPoint : NeoTrace /* Level 4 Collectible */
    {
        /// <summary>
        /// Collectible methods (NPC Level 4)
        /// </summary>
        /// <param name="e">e</param>
        /// <param name="vau">vau</param>
        /// <param name="index">index</param>
        /// <returns>bool</returns>
        public static bool PutElement(UserPoint e, NeoVersionedAppUser vau, string domain, string sindex)
        {
            if (NeoVersionedAppUser.IsNull(vau)) return false;

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            NeoStorageKey nsk = NeoStorageKey.New(vau, domain, "UserPoint");

            byte[] bkey;
            e._state = NeoEntityModel.EntityState.PUTTED;
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, bkey = NeoStorageKey.StorageKey(nsk, sindex, _bSTA), e._state.AsBigInteger());
 
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, bkey = NeoStorageKey.StorageKey(nsk, sindex, _bX), e._x); // Template: NPCLevel4APutElement_cs.txt
            Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, bkey = NeoStorageKey.StorageKey(nsk, sindex, _bY), e._y); // Template: NPCLevel4APutElement_cs.txt
            LogExt("PutElement(vau,i).UserPoint", e); // Template: NPCLevel4BGetElement_cs.txt
            return true;
        }

        /// <summary>
        /// Get an element of an array of entities from Storage based on a NeoStorageKey (NPC Level 4)
        /// </summary>
        /// <param name="vau">vau</param>
        /// <param name="index">index</param>
        /// <returns>UserPoint</returns>
        public static UserPoint GetElement(NeoVersionedAppUser vau, string domain, string sindex)
        {
            if (NeoVersionedAppUser.IsNull(vau)) return Null();

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            NeoStorageKey nsk = NeoStorageKey.New(vau, domain, "UserPoint");

            UserPoint e;
            byte[] bkey;
            byte[] bsta = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, bkey = NeoStorageKey.StorageKey(nsk, sindex, _bSTA));
            NeoTrace.Trace("Get(bkey).UserPoint.bsta", bsta.Length, bsta);
            if (bsta.Length == 0)
            {
                e = UserPoint.Missing();
            }
            else // not MISSING
            {
                /*EXT*/
                byte[] bext = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, bkey = NeoStorageKey.StorageKey(nsk, sindex, _bEXT));
                int ista = (int)bsta.AsBigInteger();
                NeoEntityModel.EntityState sta = (NeoEntityModel.EntityState)ista;
                if (sta == NeoEntityModel.EntityState.TOMBSTONED)
                {
                    e = UserPoint.Tombstone();
                }
                else // not MISSING && not TOMBSTONED
                {
                    e = new UserPoint();
                    BigInteger X = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, bkey = NeoStorageKey.StorageKey(nsk, sindex, _bX)).AsBigInteger(); // Template: NPCLevel4CGetElement_cs.txt
                    BigInteger Y = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, bkey = NeoStorageKey.StorageKey(nsk, sindex, _bY)).AsBigInteger(); // Template: NPCLevel4CGetElement_cs.txt
                    e._x = X; e._y = Y;  // NPCLevel4DBuryElement_cs.txt
                    e._state = sta;
                    e._state = NeoEntityModel.EntityState.GETTED; /* OVERRIDE */
                }
            }
            LogExt("Get(bkey).UserPoint.e", e);
            return e;
        }

        /// <summary>
        /// Bury an element of an array of entities in Storage based on a NeoStorageKey (NPC Level 4)
        /// </summary>
        /// <param name="vau">vau</param>
        /// <param name="index">index</param>
        /// <returns>UserPoint</returns>
        public static UserPoint BuryElement(NeoVersionedAppUser vau, string domain, string sindex)
        {
            if (NeoVersionedAppUser.IsNull(vau)) // TODO - create NeoEntityModel.EntityState.BADKEY?
            {
                return UserPoint.Null();
            }

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            NeoStorageKey nsk = NeoStorageKey.New(vau, domain, "UserPoint");

            byte[] bkey;
            UserPoint e;
            /*STA*/
            byte[] bsta = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, bkey = NeoStorageKey.StorageKey(nsk, sindex, _bSTA));
            NeoTrace.Trace("Bury(vau,index).UserPoint.bsta", bsta.Length, bsta);
            if (bsta.Length == 0)
            {
                e = UserPoint.Missing();
            }
            else // not MISSING - bury it
            {
                e = UserPoint.Tombstone(); // TODO - should Bury() preserve the exist field values or re-initialize them? Preserve is cheaper but not as private
                Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, bkey = NeoStorageKey.StorageKey(nsk, sindex, _bSTA), e._state.AsBigInteger());

                Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, bkey = NeoStorageKey.StorageKey(nsk, sindex, _bX), e._x); // NPCLevel4EBuryElement_cs.txt
                Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, bkey = NeoStorageKey.StorageKey(nsk, sindex, _bY), e._y); // NPCLevel4EBuryElement_cs.txt
            } // Template: NPCLevel4Part2_cs.txt
            LogExt("Bury(vau,i).UserPoint", e);
            return e;
        }
    }
}