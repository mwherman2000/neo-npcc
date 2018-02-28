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

    public class Point : NPCLevel0Basic, NPCLevel1Managed, NPCLevel2Persistable
    {
        public BigInteger x;
        public BigInteger y;
        //public string s;
        //public byte[] bytes;
        //public double d;
    }

    //public class Point2 
    //{
    //    public BigInteger x;
    //    public BigInteger y;
    //    public string s;
    //    public byte[] bytes;
    //    //public double d;
    //}
}
