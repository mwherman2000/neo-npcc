using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NPC.TestCases.T1
{
    public class Line : NPCLevel0Basic, 
                        NPCLevel1Managed, 
                        NPCLevel2Persistable, 
                        NPCLevel3Deletable, 
                        NPCLevel4Collectible
    {
        byte[] bKeyP1;
        byte[] bKeyP2;
    }
}
