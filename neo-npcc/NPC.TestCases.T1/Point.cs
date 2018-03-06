using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NPC.TestCases.T1
{
    public interface NPCLevel0Basic { }
    public interface NPCLevel1Managed { }
    public interface NPCLevel2Persistable { }
    public interface NPCLevel3Deletable { }
    public interface NPCLevel4Collectible { }
    public interface NPCLevel4CollectibleExt { }

    public class Point : NPCLevel0Basic, 
                         NPCLevel1Managed, 
                         NPCLevel2Persistable, 
                         NPCLevel3Deletable,
                         NPCLevel4Collectible,
                         NPCLevel4CollectibleExt
    {
        public BigInteger x;
        public BigInteger y;
    }
}
