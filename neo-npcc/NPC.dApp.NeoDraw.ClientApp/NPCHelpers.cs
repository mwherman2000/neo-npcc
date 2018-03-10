using Neo.Cryptography;
using NeoLux;
using NPC.dApps.NeoDraw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NPC.dApp.NeoDraw.ClientApp
{
    public static class NPCHelpers
    {
        public const string NeoDraw_ContractHash = "694ebe0840d1952b09f5435152eebbbc1f8e4b8e";
        //public const string TestNetAccount_PrivateKey = "40f0cdc78e81005fca121d433e530bd170b0b3fb3bc465cc15d717f0ebdb8674";
        public const string TestNetAccount_PrivateKey = "5d1a4f6b033b9216c4627dd9b2213826d20589e8c0f727f4b2fd9642348bd483";

        public static UserPoint[] GetAllPoints(byte[] encodedUsername)
        {
            List<UserPoint> points = new List<UserPoint>();

            var api = NeoDB.ForTestNet();
            var response = api.TestInvokeScript(NeoDraw_ContractHash, new object[] { "getall", "point", new object[] { encodedUsername } });
            object[] resultsArray = (object[])response.result;
            if (resultsArray != null)
            {
                //Console.WriteLine("resultsArray.length: " + resultsArray.Length);
                for (int element = 0; element < resultsArray.Length; element++)
                {
                    byte[] bx = (byte[])(((object[])((object[])resultsArray[element])[0])[0]);
                    byte[] by = (byte[])(((object[])((object[])resultsArray[element])[0])[1]);
                    string sx = Encoding.ASCII.GetString(bx);
                    string sy = Encoding.ASCII.GetString(by);
                    //Console.WriteLine("sx,sy: " + sx + " " + sy);
                    int ix = Int32.Parse(sx);
                    int iy = Int32.Parse(sy);
                    //Console.WriteLine("ix,iy: " + ix.ToString() + " " + iy.ToString());

                    points.Add(new UserPoint { x = ix, y = iy });
                }
            }

            return points.ToArray<UserPoint>();
        }

        public static UserCredentials GetUserCredentials(byte[] encodedUsername)
        {
            UserCredentials uc = null;

            var api = NeoDB.ForTestNet();
            //Console.WriteLine("guc.encodedUsername: '" + encodedUsername.ToHexString() + "'");
            var response2 = api.TestInvokeScript(NeoDraw_ContractHash, new object[] { "get", "user", new object[] { encodedUsername } });
            object[] resultsArray2 = (object[])response2.result;
            byte[] bencodedUsername = (byte[])(((object[])((object[])resultsArray2[0])[0])[0]);
            byte[] bencodedPassword = (byte[])(((object[])((object[])resultsArray2[0])[0])[1]);
            BigInteger status = (BigInteger)(((object[])((object[])resultsArray2[0])[0])[2]);
            //string sencodedUsername = Encoding.ASCII.GetString(bencodedUsername).Replace("\0", "");
            //string sencodedPassword = Encoding.ASCII.GetString(bencodedPassword).Replace("\0", "");
            string sencodedUsername = bencodedUsername.ToHexString();
            string sencodedPassword = bencodedPassword.ToHexString();
            //Console.WriteLine("guc.sencodedUsername,sencodedPassword: '" + sencodedUsername + "' '" + sencodedPassword + "'");
            //Console.WriteLine("guc.sencodedUsername,sencodedPassword: " + sencodedUsername.Length + " " + sencodedPassword.Length + "");
            //Console.WriteLine("guc.status: " + status.ToString());

            if (status != 5 && sencodedUsername.Length > 0 && sencodedPassword.Length > 0)
            {
                uc = new UserCredentials {  encodedUsername =  bencodedUsername, encodedPassword = bencodedPassword };
            }

            return uc;
        }

        public static bool AddUser(byte[] encodedUsername, byte[]encodedPassword)
        {


            var api = NeoDB.ForTestNet();
            var TestNetAccount_KeyPair = new KeyPair(TestNetAccount_PrivateKey.HexToBytes());
            bool response5 = api.CallContract(TestNetAccount_KeyPair, NeoDraw_ContractHash, new object[] { "add", "user", new object[] { encodedPassword, encodedUsername } });

            //Console.WriteLine("response5: " + ((bool)response5).ToString());
            return (bool)response5;
        }

        public static bool AddPoint(byte[] encodedUsername, UserPoint up)
        {
            //Console.WriteLine("AddPoint:Adding point ( " + up.x.ToString() + ", " + up.y.ToString() + ") for " + encodedUsername.ToHexString());
            var api = NeoDB.ForTestNet();
            var TestNetAccount_KeyPair = new KeyPair(TestNetAccount_PrivateKey.HexToBytes());
            bool response5 = api.CallContract(TestNetAccount_KeyPair, NeoDraw_ContractHash, new object[] { "add", "point", new object[] { up.y, up.x, encodedUsername } });

            //Console.WriteLine("response5: " + ((bool)response5).ToString());
            return (bool)response5;
        }
    }
}
