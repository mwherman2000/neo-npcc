using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mwherman2000.NPC
{
    public class NeoTrace /* Level *all* */
    {
        public static void Trace(params object[] args)
        {
            Neo.SmartContract.Framework.Services.Neo.Runtime.Notify(args);
        }
    }
}