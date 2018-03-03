using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// NPC.Runtime.NeoTrace
///
/// Generated:       2018-03-02 8:00:24 PM by npcc - NEO Class Framework (NPC) 2.0 Compiler v1.0.0.0
/// NPC Project:     https://github.com/mwherman2000/neo-npcc/blob/master/README.md
/// NPC Lead:        Michael Herman (Toronto) (mwherman@parallelspace.net)
/// </summary>

namespace NPC.Runtime
{
    public class NeoTrace /* Level *all* */
    {
        public static void Trace(params object[] args)
        {
            Neo.SmartContract.Framework.Services.Neo.Runtime.Notify(args);
        }
    }
}
