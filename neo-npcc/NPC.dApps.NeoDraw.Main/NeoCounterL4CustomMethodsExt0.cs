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
        public static BigInteger GiveBackLastNumber(NeoVersionedAppUser vau, NeoCounters counter)
        {
            NeoCounter nc = NeoCounter.GetElement(vau, DOMAINAC, (int)counter); // Get persisted counter value
            if (NeoTrace.INFO) NeoCounter.LogExt("GiveBackLastNumber", nc);

            if (NeoCounter.IsMissing(nc))
            {
                nc = NeoCounter.New(); // Create a new counter value
            }
            else // Get and decrement counter value by 1
            {
                BigInteger currentNumber = NeoCounter.GetCurrentNumber(nc);
                if (NeoTrace.INFO) NeoTraceRuntime.TraceRuntime("currentNumber", currentNumber);
                currentNumber = currentNumber - 1;
                if (NeoTrace.INFO) NeoTraceRuntime.TraceRuntime("currentNumber", currentNumber);
                NeoCounter.SetCurrentNumber(nc, currentNumber);
                if (NeoTrace.INFO) NeoCounter.LogExt("GiveBackLastNumber", nc);
                NeoCounter.PutElement(nc, vau, DOMAINAC, (int)counter); // Persist the incremented current value of the counter
                if (NeoTrace.INFO) NeoCounter.LogExt("GiveBackLastNumber", nc);
            }

            return NeoCounter.GetCurrentNumber(nc);
        }

        // Use case example: domain = user script hash, counter = NeoCounters.UserPointsCounter
        public static BigInteger GiveBackLastNumber(NeoVersionedAppUser vau, byte[] domain, NeoCounters counter)
        {
            NeoCounter nc = NeoCounter.GetElement(vau, domain, (int)counter); // Get persisted counter value
            if (NeoTrace.INFO) NeoCounter.LogExt("GiveBackLastNumber", nc);

            if (NeoCounter.IsMissing(nc))
            {
                nc = NeoCounter.New(); // Create a new counter value
            }
            else // Get and decrement counter value by 1
            {
                BigInteger currentNumber = NeoCounter.GetCurrentNumber(nc);
                if (NeoTrace.INFO) NeoTraceRuntime.TraceRuntime("currentNumber", currentNumber);
                currentNumber = currentNumber - 1;
                if (NeoTrace.INFO) NeoTraceRuntime.TraceRuntime("currentNumber", currentNumber);
                NeoCounter.SetCurrentNumber(nc, currentNumber);
                if (NeoTrace.INFO) NeoCounter.LogExt("GiveBackLastNumber", nc);
                NeoCounter.PutElement(nc, vau, domain, (int)counter); // Persist the incremented current value of the counter
                if (NeoTrace.INFO) NeoCounter.LogExt("GiveBackLastNumber", nc);
            }

            return NeoCounter.GetCurrentNumber(nc);
        }

        //// Use case example: domain = user script hash, counter = NeoCounters.UserPointsCounter
        //public static BigInteger GiveBackLastNumber(NeoVersionedAppUser vau, string domain, NeoCounters counter)
        //{
        //    NeoCounter nc = NeoCounter.GetElement(vau, domain, (int)counter); // Get persisted counter value
        //    if (NeoTrace.INFO) NeoCounter.LogExt("GiveBackLastNumber", nc);

        //    if (NeoCounter.IsMissing(nc))
        //    {
        //        nc = NeoCounter.New(); // Create a new counter value
        //    }
        //    else // Get and decrement counter value by 1
        //    {
        //        BigInteger currentNumber = NeoCounter.GetCurrentNumber(nc);
        //        if (NeoTrace.INFO) NeoTrace.Trace("currentNumber", currentNumber);
        //        currentNumber = currentNumber - 1;
        //        if (NeoTrace.INFO) NeoTrace.Trace("currentNumber", currentNumber);
        //        NeoCounter.SetCurrentNumber(nc, currentNumber);
        //        if (NeoTrace.INFO) NeoCounter.LogExt("GiveBackLastNumber", nc);
        //        NeoCounter.PutElement(nc, vau, domain, (int)counter); // Persist the incremented current value of the counter
        //        if (NeoTrace.INFO) NeoCounter.LogExt("GiveBackLastNumber", nc);
        //    }

        //    return NeoCounter.GetCurrentNumber(nc);
        //}
    }
}
