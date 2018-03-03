using System.Numerics;

namespace NPC.dApps.NeoDraw
{
    public class NeoCounter:   NPCLevel0Basic,
                        NPCLevel1Managed,
                        NPCLevel2Persistable,
                        NPCLevel3Deletable,
                        NPCLevel4Collectible
    {
        public BigInteger nextIndex = 1234;
    }
}
