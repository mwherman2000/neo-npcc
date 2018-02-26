using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NPCPoint0
{
    public interface NPCLevel0 { }
    public interface NPCLevel1 { }
    public interface NPCLevel2 { }

    public class Point : NPCLevel0, NPCLevel1, NPCLevel2
    {
        public BigInteger x;
        public BigInteger y;
    }
}
