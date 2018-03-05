using System;
using System.Numerics;

namespace NPC.dApps.NeoDraw
{
    public partial class NeoCounter:    NPCLevel0Basic,
                                        NPCLevel1Managed,
                                        NPCLevel2Persistable,
                                        NPCLevel3Deletable,
                                        NPCLevel4Collectible
    {
        public BigInteger currentNumber; // Current value of a counter. The last number given out.
    }
}
