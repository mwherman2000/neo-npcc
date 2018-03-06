using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NPC.dApps.NeoDraw
{
    public class UserPoint : NPCLevel0Basic, 
                         NPCLevel1Managed, 
                         NPCLevel2Persistable, 
                         NPCLevel3Deletable,  
                         NPCLevel4Collectible,
                         NPCLevel4CollectibleExt
    {
        public BigInteger x;
        public BigInteger y;
        public byte[] encodedUsername;
    }
}
