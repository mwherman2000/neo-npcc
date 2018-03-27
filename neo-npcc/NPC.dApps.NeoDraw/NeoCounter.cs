using System.Numerics;

namespace NPC.dApps.NeoDraw
{
    public partial class NeoCounter:    NPCLevel0Basic,
                                        NPCLevel1Managed,
                                        NPCLevel2Persistable,
                                        NPCLevel3Deletable,
                                        NPCLevel4Collectible,
                                        NPCLevel4CollectibleExt
    {
        public BigInteger currentNumber; // Next number to give out
    }
}
