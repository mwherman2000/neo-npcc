using NPC.Runtime;
using System;
using System.Numerics;
using Neo.SmartContract.Framework;

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
        private static readonly byte[] DOMAINAC = "AppCounters".AsByteArray();

        public enum NeoCounters
        {
            UserCounter,
            PointCounter,
            UserPointsCounter
        }

        public static BigInteger TakeNextNumber(NeoVersionedAppUser vau, NeoCounters counter)
        {
            NeoCounter nc = NeoCounter.GetElement(vau, DOMAINAC, (int)counter); // Get persisted counter value
            if (NeoTrace.INFO) NeoCounter.LogExt("TakeNextNumber", nc);

            if (NeoCounter.IsMissing(nc))
            {
                nc = NeoCounter.New(); // Create a new counter value
                if (NeoTrace.INFO) NeoCounter.LogExt("TakeNextNumber.putnew", nc);
                NeoCounter.PutElement(nc, vau, DOMAINAC, (int)counter); // Persist the new counter
            }
            else // Get and increment counter value by 1
            {
                BigInteger newNumber = NeoCounter.GetCurrentNumber(nc);
                if (NeoTrace.INFO) NeoTraceRuntime.TraceRuntime("newNumber", newNumber);
                newNumber = newNumber + 1;
                if (NeoTrace.INFO) NeoTraceRuntime.TraceRuntime("newNumber", newNumber);
                NeoCounter.SetCurrentNumber(nc, newNumber);
                if (NeoTrace.INFO) NeoCounter.LogExt("TakeNextNumber.putincr", nc);
                NeoCounter.PutElement(nc, vau, DOMAINAC, (int)counter); // Persist the new counter
            }

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

        // Use case example: domain = user script hash, counter = NeoCounters.UserPointsCounter
        // TakeNextNumber semantics: return the current number and then advance the counter ...like at the grocery store
        public static BigInteger TakeNextNumber(NeoVersionedAppUser vau, byte[] domain, NeoCounters counter)
        {
            NeoCounter nc = NeoCounter.GetElement(vau, domain, (int)counter); // Get persisted counter value
            if (NeoTrace.INFO) NeoCounter.LogExt("TakeNextNumber", nc);

            if (NeoCounter.IsMissing(nc)) // Create persist the new counter entity
            {
                if (NeoTrace.INFO) NeoCounter.LogExt("TakeNextNumber.domain and counter is missing", nc);
                nc = NeoCounter.New(); // Create a new counter entity
                if (NeoTrace.INFO) NeoCounter.LogExt("TakeNextNumber.putnew", nc);
                NeoCounter.PutElement(nc, vau, domain, (int)counter); // Persist the new counter entity with a value of zero
            }

            if (NeoTrace.INFO) NeoCounter.LogExt("TakeNextNumber.exists", nc);
            BigInteger currentNextNumber = NeoCounter.GetCurrentNumber(nc);
            if (NeoTrace.INFO) NeoTraceRuntime.TraceRuntime("currentNextNumber", currentNextNumber);
            BigInteger newNextNumber  = currentNextNumber + 1;
            if (NeoTrace.INFO) NeoTraceRuntime.TraceRuntime("nextNumber", newNextNumber);
            NeoCounter.SetCurrentNumber(nc, newNextNumber);
            if (NeoTrace.INFO) NeoCounter.LogExt("TakeNextNumber.putincr", nc);
            NeoCounter.PutElement(nc, vau, domain, (int)counter); // Persist the new counter

            return currentNextNumber;
        }

        public static BigInteger GetCurrentNextNumber(NeoVersionedAppUser vau, byte[] domain, NeoCounters counter)
        {
            BigInteger result = -1;

            NeoCounter nc = NeoCounter.GetElement(vau, domain, (int)counter); // Get persisted counter value
            if (!NeoCounter.IsMissing(nc))
            {
                result = NeoCounter.GetCurrentNumber(nc);
            }

            return result; // Return the current value for this counter
        }
    }
}
