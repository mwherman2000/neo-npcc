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
        const string SmartContractName = "NPC.dApps.NeoDraw.Main" + " " + "v1.0.3";

        static readonly byte[] DOMAINUCD = "UCD".AsByteArray(); // User Credentials Directory (UCD)
        static readonly byte[] DOMAINUDP = "UCP".AsByteArray(); // User Drawing Points (UCP)

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
            object[] results = { -1 };

            //NeoVersionedAppUser AppVAU = NeoVersionedAppUser.New("NPC.dApps.NeoDraw.Main", 1, 1, 1, NeoEntityModel.GetInvokingAddressScriptHash());
            //NeoVersionedAppUser AppVAU = NeoVersionedAppUser.New("NPC.dApps.NeoDraw.Main", 1, 1, 1, ExecutionEngine.ExecutingScriptHash);
            NeoVersionedAppUser AppVAU = NeoVersionedAppUser.New("NPC.dApps.NeoDraw.Main", 1, 1, 1, "7074acf3f06dd3f456e11053ebf61c5b04b07ebc".AsByteArray()); // DEBUG ONLY
            if (NeoTrace.TESTING) NeoVersionedAppUser.LogExt("AppVAU", AppVAU);

            //byte[] ownerScriptHash = NeoEntityModel.GetInvokingAddressScriptHash();
            //if (NeoTrace.INFO) NeoTrace.Trace("ownerScriptHash", ownerScriptHash);
            //NeoVersionedAppUser.SetUserScriptHash(AppVAU, ownerScriptHash);
            //if (NeoTrace.INFO) NeoVersionedAppUser.LogExt("AppVAU", AppVAU);

            NeoDrawEntityType entityType = NeoDrawEntityType.UnknownEntityType;

            if (NeoTrace.SPLASH) NeoTrace.Trace(""); 
            if (NeoTrace.SPLASH) NeoTrace.Trace(SmartContractName + " ********************");
            if (NeoTrace.ARGSRESULTS) NeoTrace.LogExt();
            if (NeoTrace.ARGSRESULTS) NeoTrace.Trace("operation", operation);
            if (NeoTrace.ARGSRESULTS) NeoTrace.Trace("entity", entity);
            if (entity == "user") { entityType = NeoDrawEntityType.User; }
            else if (entity == "point") { entityType = NeoDrawEntityType.Point; }
            else
            {
                message = "unknown entity type '" + entity + "'";
                if (NeoTrace.ERROR) NeoTrace.Trace("**ERROR** entity", message);
                entityType = NeoDrawEntityType.UnknownEntityType;
                success = false;
            }
            if (NeoTrace.INFO) NeoTrace.Trace("entityType", (BigInteger)((int)entityType));

            if (success)
            {
                if (operation == "add")
                {
                    if (entityType == NeoDrawEntityType.User)
                    {
                        if (NeoTrace.INFO) NeoTrace.Trace("add", "user");
                        results = UserAdd(AppVAU, args);
                    }
                    else if (entityType == NeoDrawEntityType.Point)
                    {
                        if (NeoTrace.INFO) NeoTrace.Trace("add", "point");
                        results = PointAdd(AppVAU, args);
                    }
                }
                else if (operation == "delete")
                {
                    if (entityType == NeoDrawEntityType.User)
                    {
                        if (NeoTrace.INFO) NeoTrace.Trace("delete", "user");
                        results = UserDelete(AppVAU, args);
                    }
                    else if (entityType == NeoDrawEntityType.Point)
                    {
                        if (NeoTrace.INFO) NeoTrace.Trace("delete", "point");
                        results = PointDeleteLast(AppVAU, args);
                    }
                }
                else if (operation == "get")
                {
                    if (entityType == NeoDrawEntityType.User)
                    {
                        if (NeoTrace.INFO) NeoTrace.Trace("get", "user");
                        results = UserGetSingle(AppVAU, args);
                    }
                    else if (entityType == NeoDrawEntityType.Point)
                    {
                        if (NeoTrace.INFO) NeoTrace.Trace("get", "point");
                        results = PointGetSingle(AppVAU, args);
                    }
                }
                else if (operation == "getall")
                {
                    if (entityType == NeoDrawEntityType.User)
                    {
                        if (NeoTrace.INFO) NeoTrace.Trace("getall", "user");
                        results = UserGetAll(AppVAU, args);
                    }
                    else if (entityType == NeoDrawEntityType.Point)
                    {
                        if (NeoTrace.INFO) NeoTrace.Trace("getall", "point");
                        results = PointGetAll(AppVAU, args);
                    }
                }
                else
                {
                    if (NeoTrace.INFO) NeoTrace.Trace("Unknown operation", operation);
                    message = "unknown operation '" + operation + "'";
                    if (NeoTrace.ERROR) NeoTrace.Trace("**ERROR** operation", message);
                    success = false;
                }
            }

            if (NeoTrace.ARGSRESULTS) NeoTrace.Trace("Main.results", results);

            return results;
        }

        private static object[] PointGetSingle(NeoVersionedAppUser AppVAU, object[] args)
        {
            throw new NotImplementedException();
        }

        private static object[] UserGetSingle(NeoVersionedAppUser AppVAU, object[] args)
        {
            object[] results = { -1 };

            byte[] encodedUsername = (byte[])args[0];
            if (NeoTrace.ARGSRESULTS) NeoTrace.Trace("UserGetSingle.encodedUsername", encodedUsername);

            UserCredentials uc = FindUser(AppVAU, encodedUsername);
            if (NeoTrace.INFO) UserCredentials.LogExt("UserGetSingle.uc", uc);

            results = new object[] { uc };
            if (NeoTrace.ARGSRESULTS) NeoTrace.Trace("UserGetSingle.results", results);

            return results;
        }

        private static object[] PointGetAll(NeoVersionedAppUser AppVAU, object[] args)
        {
            object[] results = { -1 };

            byte[] encodedUsername = (byte[])args[0];
            if (NeoTrace.ARGSRESULTS) NeoTrace.Trace("PointGetAll.encodedUsername", encodedUsername);

            UserCredentials uc = FindUser(AppVAU, encodedUsername);
            if (UserCredentials.IsMissing(uc))
            {
                if (NeoTrace.INFO) UserCredentials.LogExt("PointGetAll.user missing", uc);
            }
            else
            {
                if (NeoTrace.INFO) UserCredentials.LogExt("PointGetAll.user exists", uc);
                BigInteger nPoints = NeoCounter.GetCurrentNextNumber(AppVAU, encodedUsername, NeoCounters.UserPointsCounter);
                if(NeoTrace.INFO) NeoTrace.Trace("PointGetAll.nPoints", nPoints);

                UserPoint[] points = new UserPoint[(int)nPoints];
                for (int index = 0; index < nPoints; index++)
                {
                    if (NeoTrace.INFO) NeoTrace.Trace("PointGetAll.index", index);
                    UserPoint up = UserPoint.GetElement(AppVAU, encodedUsername, (int)index);
                    if (NeoTrace.INFO) UserPoint.LogExt("PointGetAll.up", up);
                    points[index] = up;
                }
                results = points;
            }

            if (NeoTrace.ARGSRESULTS) NeoTrace.Trace("PointGetAll.results", results);

            return results;
        }

        private static object[] UserGetAll(NeoVersionedAppUser AppVAU, object[] args)
        {
            throw new NotImplementedException();
        }

        private static object[] PointDeleteLast(NeoVersionedAppUser AppVAU, object[] args)
        {
            object[] results = { -1 };

            if (NeoTrace.INFO) NeoVersionedAppUser.LogExt("PointDeleteLast.AppVAU", AppVAU);

            byte[] encodedUsername = (byte[])args[0];
            if (NeoTrace.ARGSRESULTS) NeoTrace.Trace("PointDeleteLast.encodedUsername", encodedUsername);

            UserCredentials uc = FindUser(AppVAU, encodedUsername);
            if (UserCredentials.IsMissing(uc))
            {
                if (NeoTrace.INFO) UserCredentials.LogExt("PointDeleteLast.user missing", uc);
            }
            else
            {
                if (NeoTrace.INFO) UserCredentials.LogExt("PointDeleteLast.user exists", uc);
                BigInteger nPoints = NeoCounter.GiveBackLastNumber(AppVAU, encodedUsername, NeoCounters.UserPointsCounter);
                if (NeoTrace.INFO) NeoTrace.Trace("PointDeleteLast.nPoints", nPoints);

                results = new object[] { nPoints };
            }

            if (NeoTrace.ARGSRESULTS) NeoTrace.Trace("PointDeleteLast.results", results);

            return results;
        }

        private static object[] UserDelete(NeoVersionedAppUser AppVAU, object[] args)
        {
            throw new NotImplementedException();
        }

        private static object[] PointAdd(NeoVersionedAppUser AppVAU, object[] args)
        {
            object[] results = { -1 };

            if (NeoTrace.ARGSRESULTS) NeoVersionedAppUser.LogExt("PointAdd.AppVAU", AppVAU);

            byte[] encodedUsername = (byte[])args[0];
            if (NeoTrace.ARGSRESULTS) NeoTrace.Trace("PointAdd.encodedUsername", encodedUsername);
            BigInteger x = (BigInteger)(args[1]);
            if (NeoTrace.ARGSRESULTS) NeoTrace.Trace("PointAdd.x", x);
            BigInteger y = (BigInteger)(args[2]);
            if (NeoTrace.ARGSRESULTS) NeoTrace.Trace("PointAdd.y", y);

            UserCredentials uc = FindUser(AppVAU, encodedUsername);
            if (UserCredentials.IsMissing(uc))
            {
                if (NeoTrace.INFO) UserCredentials.LogExt("PointAdd.user missing", uc);
            }
            else
            {
                if (NeoTrace.INFO) UserCredentials.LogExt("PointAdd.user exists", uc);

                UserPoint up = UserPoint.New(x, y);
                BigInteger index = NeoCounter.TakeNextNumber(AppVAU, encodedUsername, NeoCounters.UserPointsCounter);
                if (NeoTrace.INFO) NeoTrace.Trace("PointAdd.index", index);
                UserPoint.PutElement(up, AppVAU, encodedUsername, (int)index);

                results = new object[] { index };
            }

            if (NeoTrace.ARGSRESULTS) NeoTrace.Trace("PointAdd.results", results);

            return results;
        }

        private static object[] UserAdd(NeoVersionedAppUser AppVAU, object[] args)
        {
            object[] results = { -1 };

            byte[] encodedUsername = (byte[])args[0];
            if (NeoTrace.ARGSRESULTS) NeoTrace.Trace("UserAdd.encodedUsername", encodedUsername);
            byte[] encodedPassword = (byte[])args[1];
            if (NeoTrace.ARGSRESULTS) NeoTrace.Trace("UserAdd.encodedPassword", encodedPassword);

            UserCredentials uc = FindUser(AppVAU, encodedUsername);

            if (UserCredentials.IsMissing(uc)) // add the unique new user
            {
                if (NeoTrace.INFO) UserCredentials.LogExt("UserAdd.missing", uc);
                uc = UserCredentials.New(encodedUsername, encodedPassword);
                UserCredentials.PutElement(uc, AppVAU, DOMAINUCD, encodedUsername);
                if (NeoTrace.INFO) UserCredentials.LogExt("UserAdd.added", uc);
            }
            else
            {
                if (NeoTrace.INFO) UserCredentials.LogExt("UserAdd.exists", uc);
            }

            results = new object[] { uc };
            if (NeoTrace.ARGSRESULTS) NeoTrace.Trace("UserAdd.results", results);

            return results;
        }

        private static UserCredentials FindUser(NeoVersionedAppUser AppVAU, byte[] encodedUsername)
        {
            if (NeoTrace.ARGSRESULTS) NeoTrace.Trace("FindUser.encodedUsername", encodedUsername);

            UserCredentials result = UserCredentials.GetElement(AppVAU, DOMAINUCD, encodedUsername);

            if (NeoTrace.ARGSRESULTS) UserCredentials.LogExt("FindUser.result", result);

            return result;
        }

        //private static UserCredentials FindUser(NeoVersionedAppUser AppVAU, string encodedUsername)
        //{
        //    UserCredentials result = UserCredentials.Missing();
        //    if (NeoTrace.INFO) NeoTrace.Trace("FindUser.encodedUsername", encodedUsername);
        //    UserCredentials uc = UserCredentials.GetElement(AppVAU, DOMAINUCD, encodedUsername);
        //    if (NeoTrace.INFO) UserCredentials.LogExt("FindUser.uc", uc);

        //    result = uc;

        //    return result;
        //}
    }
}
