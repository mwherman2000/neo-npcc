using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using NPC.Runtime;
using System;
using System.Numerics;

namespace NPC.dApps.NeoDraw.Main
{
    public class Contract1 : SmartContract
    {
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

            NeoVersionedAppUser AppVAU = NeoVersionedAppUser.New("NPC.dApps.NeoDraw.Main".AsByteArray(), 1, 1, 1, NeoEntityModel.GetInvokingAddressScriptHash());
        
            NeoVersionedAppUser.LogExt("AppVAU", AppVAU);
            byte[] ownerScriptHash = NeoEntityModel.GetInvokingAddressScriptHash();
            NeoTrace.Trace("ownerScriptHash", ownerScriptHash);
            NeoVersionedAppUser.SetUserScriptHash(AppVAU, ownerScriptHash);
            NeoVersionedAppUser.LogExt("AppVAU", AppVAU);

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
                        results = PointDelete(AppVAU, args);
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
            UserCredentials uc = UserCredentials.Null();

            byte[] encodedUsername = (byte[])args[0];
            NeoTrace.Trace("encodedUsername", encodedUsername);

            int indexMax = (int)NeoCounter.GetNextNumber(AppVAU, NeoCounter.NeoCounters.UserCounter);
            NeoTrace.Trace("UserGetSingle.indexMax", indexMax);

            if (indexMax != -1)
            {
                for (int index = 0; index < indexMax; index++) // TODO - performance
                {
                    NeoTrace.Trace("UserGetSingle.index", index);
                    uc = UserCredentials.GetElement(AppVAU, index);
                    if (!UserCredentials.IsMissing(uc))
                    {
                        NeoTrace.Trace("UserGetSingle.!IsMissing(uc)", encodedUsername);
                        if (UserCredentials.GetEncodedUsername(uc) == encodedUsername) break;
                    }
                    else
                    {
                        NeoTrace.Trace("UserGetSingle.IsMissing(uc)", index);
                    }
                }
            }

            results = new object[] { uc };
            return results;
        }

        private static object[] PointGetAll(NeoVersionedAppUser AppVAU, object[] args)
        {
            throw new NotImplementedException();
        }

        private static object[] UserGetAll(NeoVersionedAppUser AppVAU, object[] args)
        {
            throw new NotImplementedException();
        }

        private static object[] PointDelete(NeoVersionedAppUser AppVAU, object[] args)
        {
            throw new NotImplementedException();
        }

        private static object[] UserDelete(NeoVersionedAppUser AppVAU, object[] args)
        {
            throw new NotImplementedException();
        }

        private static object[] PointAdd(NeoVersionedAppUser AppVAU, object[] args)
        {
            throw new NotImplementedException();
        }

        private static object[] UserAdd(NeoVersionedAppUser AppVAU, object[] args)
        {
            string message = "UserAdd";
            object[] results = { 0x0 };

            byte[] encodedUsername = (byte[])args[0];
            byte[] encodedPassword = (byte[])args[1];
            NeoTrace.Trace("encodedUsername", encodedUsername);
            NeoTrace.Trace("encodedPassword", encodedPassword);

            int index = (int)NeoCounter.TakeNextNumber(AppVAU, NeoCounter.NeoCounters.UserCounter);

            UserCredentials uc = UserCredentials.New(encodedUsername, encodedPassword);
            UserCredentials.PutElement(uc, AppVAU, index);

            results = new object[] { message };
            return results;
        }
    }
}
