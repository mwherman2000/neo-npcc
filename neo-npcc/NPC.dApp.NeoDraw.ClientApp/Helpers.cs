using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NPC.dApp.NeoDraw.ClientApp
{
    public static class Helpers
    {
        static SHA256 mySHA256 = SHA256Managed.Create();

        public static byte[] GetHash(string s)
        {
            byte[] hash = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(s));
            return hash;
        }
    }
}
