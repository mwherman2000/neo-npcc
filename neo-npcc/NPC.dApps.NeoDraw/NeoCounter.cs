﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NPC.dApps.NeoDraw
{
    class NeoCounter:   NPCLevel0Basic,
                        NPCLevel1Managed,
                        NPCLevel2Persistable,
                        NPCLevel3Deletable,
                        NPCLevel4Collectible
    {
        public BigInteger nextIndex;
    }
}
