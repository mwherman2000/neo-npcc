using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework.Services.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// NPC.Runtime.NeoEntityModel
///
/// Generated:       2018-03-03 9:06:59 AM by npcc - NEO Class Framework (NPC) 2.0 Compiler v1.0.0.0
/// NPC Project:     https://github.com/mwherman2000/neo-npcc/blob/master/README.md
/// NPC Lead:        Michael Herman (Toronto) (mwherman@parallelspace.net)
/// </summary>

namespace NPC.Runtime
{
    public static class NeoEntityModel /* Level 4 Collectible */
    {
        public enum EntityState
        {
            NULL, INIT, SET, PUTTED, GETTED, MISSING, TOMBSTONED, NOTAUTHORIZED /* Future Work*/
        }

        public static BigInteger AsBigInteger(this EntityState state)
        {
            int istate = (int)state;
            BigInteger bis = istate;
            return bis;
        }
        public static EntityState BytesToEntityState(byte[] bsta)
        {
            int ista = (int)bsta.AsBigInteger();
            NeoEntityModel.EntityState sta = (NeoEntityModel.EntityState)ista;
            return sta;
        }

        public static byte[] GetInvokingAddressScriptHash()
        {
            Transaction tx = (Transaction)ExecutionEngine.ScriptContainer;
            TransactionOutput[] refs = tx.GetReferences();
            foreach (TransactionOutput r in refs)
            {
                return r.ScriptHash;
            }
            return new byte[] { 0x0 };
        }

        public static readonly byte[] NullScriptHash = "".ToScriptHash();
        public static readonly byte[] NullByteArray = { 0x0 };
    }
}
