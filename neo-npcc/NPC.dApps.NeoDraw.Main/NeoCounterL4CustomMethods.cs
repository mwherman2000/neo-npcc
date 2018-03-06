using NPC.Runtime;
using System;
using System.Numerics;

/// <summary>
/// NPC.dApps.NeoDraw.Main.NeoCounter Custom Methods
///
/// Processed:       2018-03-04 2:09:47 PM by npcc - NEO Class Framework (NPC) 2.0 Compiler v1.0.0.0
/// NPC Project:     https://github.com/mwherman2000/neo-npcc/blob/master/README.md
/// NPC Lead:        Michael Herman (Toronto) (mwherman@parallelspace.net)
/// </summary>

namespace NPC.dApps.NeoDraw.Main
{
    public partial class NeoCounter    
    {
        private const string DOMAINAC = "AppCounters";
        public enum NeoCounters
        {
            UserCounter,
            PointCounter
        }

        public static BigInteger TakeNextNumber(NeoVersionedAppUser vau, NeoCounters counter)
        {
            NeoCounter nc = NeoCounter.GetElement(vau, DOMAINAC, (int)counter); // Get persisted counter value
            NeoCounter.LogExt("TakeNextNumber", nc);

            if (NeoCounter.IsMissing(nc))
            {
                nc = NeoCounter.New(); // Create a new counter value
            }
            else // Get and increment counter value by 1
            {
                BigInteger newNumber = NeoCounter.GetCurrentNumber(nc);
                NeoTrace.Trace("newNumber", newNumber);
                newNumber = newNumber + 1;
                NeoTrace.Trace("newNumber", newNumber);
                NeoCounter.SetCurrentNumber(nc, newNumber);
                NeoCounter.LogExt("TakeNextNumber", nc);
            }

            NeoCounter.PutElement(nc, vau, DOMAINAC, (int)counter); // Persist the incremented current value of the counter
            NeoCounter.LogExt("TakeNextNumber", nc);

            return NeoCounter.GetCurrentNumber(nc);
        }

        public static BigInteger GetNextNumber(NeoVersionedAppUser vau, NeoCounters counter)
        {
            BigInteger result = -1;

            NeoCounter nc = NeoCounter.GetElement(vau, DOMAINAC, (int)counter); // Get persisted counter value
            if (!NeoCounter.IsMissing(nc))
            {
                result = NeoCounter.GetCurrentNumber(nc);
            }

            return result; // Return the current value for this counter
        }
    }
}
