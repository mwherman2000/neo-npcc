using System;
using System.Numerics;

/// <summary>
/// #NAMESPACE#.#CLASSNAME# Custom Methods
///
/// Processed:       #NOWDATETIME# by #PROGRAMNAME# v#PROGRAMVERSION#
/// NPC Project:     https://github.com/mwherman2000/neo-npcc/blob/master/README.md
/// NPC Lead:        Michael Herman (Toronto) (mwherman@parallelspace.net)
/// </summary>

namespace NPC.dApps.NeoDraw
{
    public partial class NeoCounter:    NPCLevel4CustomMethods
    {
        public enum NeoCounters
        {
            UserCounter,
            PointCounter
        }

        public static void TakeNumber(NeoCounter e, NeoCounters counter)
        {
            throw new NotImplementedException();
        }
    }
}
