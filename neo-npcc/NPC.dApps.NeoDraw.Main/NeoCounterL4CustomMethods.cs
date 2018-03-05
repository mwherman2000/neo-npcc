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
        public enum NeoCounters
        {
            UserCounter,
            PointCounter
        }

        public static BigInteger TakeNextNumber(NeoVersionedAppUser vau, NeoCounters counter)
        {
            NeoCounter nc = NeoCounter.GetElement(vau, (int)counter); // Get persisted counter value
            if (NeoCounter.IsMissing(nc))
            {
                nc = NeoCounter.New(); // Create a new counter value
            }
            else // Get and increment counter value by 1
            {
                NeoCounter.SetCurrentNumber(nc, NeoCounter.GetCurrentNumber(nc) + 1);
            }

            NeoCounter.PutElement(nc, vau, (int)counter); // Persist the current value of the counter

            return NeoCounter.GetCurrentNumber(nc); // Return the current value for this counter
        }
    }
}
