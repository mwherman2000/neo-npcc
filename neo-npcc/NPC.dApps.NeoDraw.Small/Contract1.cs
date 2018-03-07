//using Neo.Cryptography;
using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework.Services.System;
//using Neo.VM;
using NPC.Runtime;
using System;
using System.Numerics;

namespace NPC.dApps.NeoDraw.Main
{
    public class Contract1 : SmartContract
    {
        public static object[] Main(string operation, string entity, object[] args)
        {
            //byte[] eu = "1234".AsByteArray();
            //byte[] ep = "4567".AsByteArray();
            //NeoTrace.Trace("UserPoint.x,y", eu, ep);
            //UserCredentials uc = UserCredentials.New(eu, ep);
            //NeoTrace.Trace("UserPoint.x,y", eu, ep);
            //UserCredentials.LogExt("UserPoint.uc", uc);

            int ix = 1;
            int iy = 2;
            NeoTrace.Trace("UserPoint.ix,iy", ix, iy);
            UserPoint up0 = UserPoint.New(ix, iy);
            NeoTrace.Trace("UserPoint.ix,iy", ix, iy);
            UserPoint.LogExt("UserPoint.up0", up0);

            BigInteger x = 1;
            BigInteger y = 2;
            NeoTrace.Trace("UserPoint.x,y", x, y);
            UserPoint up1 = UserPoint.New((int)x, (int)y);
            NeoTrace.Trace("UserPoint.x,y", x, y);
            UserPoint.LogExt("UserPoint.up1", up1);

            NeoTrace.Trace("UserPoint.x,y", 1, 2);
            UserPoint up2 = UserPoint.New(1, 2);
            NeoTrace.Trace("UserPoint.x,y", 1, 2);
            UserPoint.LogExt("UserPoint.up2", up2);

            return new object[] { 0x3 };
        }
    }
}
