//using Neo.Cryptography;
using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework.Services.System;
//using Neo.VM;
using NPC.Runtime;
using System;
using System.Numerics;
using static NPC.dApps.NeoDraw.Main.NeoCounter;

namespace NPC.dApps.NeoDraw.Main
{
    public class Contract1 : SmartContract
    {
        const string DOMAINUCD = "UCD"; // User Credentials Directory (UCD)
        const string DOMAINUDP = "UCP"; // User Drawing Points (UCP)

        public enum NeoDrawEntityType
        {
            User,
            Point,
            UnknownEntityType
        }

        public static object[] Main(string operation, string entity, object[] args)
        {
            bool success = true;
            string message = "";
            object[] results = { 0x0 };

            //NeoVersionedAppUser AppVAU = NeoVersionedAppUser.New("NPC.dApps.NeoDraw.Main", 1, 1, 1, NeoEntityModel.GetInvokingAddressScriptHash());
            NeoVersionedAppUser AppVAU = NeoVersionedAppUser.New("NPC.dApps.NeoDraw.Main", 1, 1, 1, ExecutionEngine.ExecutingScriptHash);
            NeoVersionedAppUser.LogExt("AppVAU", AppVAU);

            //byte[] ownerScriptHash = NeoEntityModel.GetInvokingAddressScriptHash();
            //NeoTrace.Trace("ownerScriptHash", ownerScriptHash);
            //NeoVersionedAppUser.SetUserScriptHash(AppVAU, ownerScriptHash);
            //NeoVersionedAppUser.LogExt("AppVAU", AppVAU);

            NeoDrawEntityType entityType = NeoDrawEntityType.UnknownEntityType; ;
            NeoTrace.Trace("operation", operation);
            NeoTrace.Trace("entity", entity);
            if (entity == "user" || entity == "u") { entityType = NeoDrawEntityType.User; }
            else if (entity == "point" || entity == "p") { entityType = NeoDrawEntityType.Point; }
            else
            {
                entityType = NeoDrawEntityType.UnknownEntityType;
                message = "unknown entity type '" + entity + "'";
                success = false;
            }
            NeoTrace.Trace("entityType", (BigInteger)((int)entityType));

            // return results; // 0.463 Gas

            if (success)
            {
                if (operation == "add")
                {
                    if (entityType == NeoDrawEntityType.User)
                    {
                        results = UserAdd(AppVAU, args);
                    }
                    else if (entityType == NeoDrawEntityType.Point)
                    {
                        results = PointAdd(AppVAU, args);
                    }
                }
                else if (operation == "delete")
                {
                    if (entityType == NeoDrawEntityType.User)
                    {
                        results = UserDelete(AppVAU, args);
                    }
                    else if (entityType == NeoDrawEntityType.Point)
                    {
                        results = PointDeleteLast(AppVAU, args);
                    }
                }
                else if (operation == "get")
                {
                    if (entityType == NeoDrawEntityType.User)
                    {
                        results = UserGetSingle(AppVAU, args);
                    }
                    else if (entityType == NeoDrawEntityType.Point)
                    {
                        results = PointGetSingle(AppVAU, args);
                    }
                }
                else if (operation == "getall")
                {
                    if (entityType == NeoDrawEntityType.User)
                    {
                        results = UserGetAll(AppVAU, args);
                    }
                    else if (entityType == NeoDrawEntityType.Point)
                    {
                        results = PointGetAll(AppVAU, args);
                    }
                }
                else
                {
                    message = "unknown operation '" + operation + "'";
                    NeoTrace.Trace("**ERROR**", message);
                    success = false;
                }
            }

            if (!success)
            {
                results = new object[] { message };
            }
            else
            {
                // TODO
            }

            return results;
        }

        private static object[] PointGetSingle(NeoVersionedAppUser AppVAU, object[] args)
        {
            throw new NotImplementedException();
        }

        private static object[] UserGetSingle(NeoVersionedAppUser AppVAU, object[] args)
        {
            string message = "UserGetSingle";
            object[] results = { 0x0 };

            byte[] encodedUsername = (byte[])args[0];
            NeoTrace.Trace("encodedUsername", encodedUsername);

            UserCredentials uc = FindUser(AppVAU, encodedUsername);

            results = new object[] { uc };
            return results;
        }

        private static object[] PointGetAll(NeoVersionedAppUser AppVAU, object[] args)
        {
            string message = "PointGetAll";
            object[] results = { 0x0 };

            NeoVersionedAppUser.LogExt("PointGetAll.AppVAU", AppVAU);

            byte[] encodedUsername = (byte[])args[0];
            NeoTrace.Trace("PointGetAll.encodedUsername", encodedUsername);
            UserCredentials uc = FindUser(AppVAU, encodedUsername);
            if (UserCredentials.IsMissing(uc))
            {
                UserCredentials.LogExt("PointGetAll.user missing", uc);
            }
            else
            {
                UserCredentials.LogExt("PointGetAll.user exists", uc);
                BigInteger nPoints = NeoCounter.GetNextNumber(AppVAU, encodedUsername, NeoCounters.UserPointsCounter);
                UserPoint[] points = new UserPoint[(int)nPoints];
                for (int index = 0; index < nPoints; index++)
                {
                    UserPoint up = UserPoint.GetElement(AppVAU, encodedUsername, (int)index);
                    points[index] = up;
                }
                results = points;
            }

            return results;
        }

        private static object[] UserGetAll(NeoVersionedAppUser AppVAU, object[] args)
        {
            throw new NotImplementedException();
        }

        private static object[] PointDeleteLast(NeoVersionedAppUser AppVAU, object[] args)
        {
            string message = "PointDeleteLast";
            object[] results = { 0x0 };

            NeoVersionedAppUser.LogExt("PointDeleteLast.AppVAU", AppVAU);

            byte[] encodedUsername = (byte[])args[0];
            NeoTrace.Trace("PointDeleteLast.encodedUsername", encodedUsername);
            UserCredentials uc = FindUser(AppVAU, encodedUsername);
            if (UserCredentials.IsMissing(uc))
            {
                UserCredentials.LogExt("PointDeleteLast.user missing", uc);
            }
            else
            {
                UserCredentials.LogExt("PointDeleteLast.user exists", uc);
                BigInteger nPoints = NeoCounter.GiveBackLastNumber(AppVAU, encodedUsername, NeoCounters.UserPointsCounter);
                results = new object[] { nPoints };
            }

            return results;
        }

        private static object[] UserDelete(NeoVersionedAppUser AppVAU, object[] args)
        {
            throw new NotImplementedException();
        }

        private static object[] PointAdd(NeoVersionedAppUser AppVAU, object[] args)
        {
            string message = "PointAdd";
            object[] results = { 0x0 };

            NeoVersionedAppUser.LogExt("PointAdd.AppVAU", AppVAU);

            byte[] encodedUsername = (byte[])args[0];
            NeoTrace.Trace("PointAdd.encodedUsername", encodedUsername);
            UserCredentials uc = FindUser(AppVAU, encodedUsername);
            if (UserCredentials.IsMissing(uc))
            {
                UserCredentials.LogExt("PointAdd.user missing", uc);
            }
            else
            {
                UserCredentials.LogExt("PointAdd.user exists", uc);
                BigInteger x = (BigInteger)args[1];
                BigInteger y = (BigInteger)args[2];
                NeoTrace.Trace("PointAdd.x,y", x, y);
                UserPoint up = UserPoint.New(x, y,encodedUsername);
                UserPoint.LogExt("PointAdd", up);
                BigInteger index = NeoCounter.TakeNextNumber(AppVAU, encodedUsername, NeoCounters.UserPointsCounter);
                UserPoint.PutElement(up, AppVAU, encodedUsername, (int)index);
                results = new object[] { index };
            }

            return results;
        }

        private static object[] UserAdd(NeoVersionedAppUser AppVAU, object[] args)
        {
            string message = "UserAdd";
            object[] results = { 0x0 };

            NeoVersionedAppUser.LogExt("UserAdd.AppVAU", AppVAU);

            byte[] encodedUsername = (byte[])args[0];
            byte[] encodedPassword = (byte[])args[1];
            NeoTrace.Trace("UserAdd.encodedUsername", encodedUsername);
            NeoTrace.Trace("UserAdd.encodedPassword", encodedPassword);

            UserCredentials uc = FindUser(AppVAU, encodedUsername);

            if (UserCredentials.IsMissing(uc)) // add the unique new user
            { 
                uc = UserCredentials.New(encodedUsername, encodedPassword);
                UserCredentials.LogExt("UserAdd.added", uc);
                UserCredentials.PutElement(uc, AppVAU, DOMAINUCD, encodedUsername);
            }
            else
            {
                UserCredentials.LogExt("UserAdd.exists", uc);
            }

            results = new object[] { uc };
            return results;
        }

        private static UserCredentials FindUser(NeoVersionedAppUser AppVAU, byte[] encodedUsername)
        {
            UserCredentials result = UserCredentials.Missing();

            UserCredentials uc = UserCredentials.GetElement(AppVAU, DOMAINUCD, encodedUsername);

            result = uc;

            return result;
        }
    }
}
