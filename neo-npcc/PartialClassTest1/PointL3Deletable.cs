using NPC.Runtime;
using Neo.SmartContract.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PartialClassTest1
{
    public partial class Point : NeoTrace /* Level 3 Deletable */
    {
        // Deletable methods
        public static bool IsBuried(Point p)
        {
            return (p._state == NeoEntityModel.EntityState.TOMBSTONED);
        }

        public static Point Tombstone()
        {
            Point p = new Point();
            p._x = 0; p._y = 0;
            p._state = NeoEntityModel.EntityState.TOMBSTONED;
            LogExt("Tombstone().p", p);
            return p;
        }

        public static Point Bury(byte[] key)
        {
            if (key.Length == 0) return Null();

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            byte[] _bkeyTag = Helper.Concat(key, _bclassKeyTag);

            Point p;
            byte[] bsta = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, Helper.Concat(_bkeyTag, _bSTA));
            NeoTrace.Trace("Bury(bkey).bs", bsta.Length, bsta);
            if (bsta.Length == 0)
            {
                p = Point.Missing();
            }
            else // not MISSING - bury it
            {
                p = Point.Tombstone(); // but don't overwrite existing field values - just tombstone it
                Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, Helper.Concat(_bkeyTag, _bSTA), p._state.AsBigInteger());
                //Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, Helper.Concat(_bkeyTag, _bX), p._x);
                //Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, Helper.Concat(_bkeyTag, _bY), p._y);
            }
            LogExt("Bury(bkey).p", p);
            return p; // return Point p to signal if key is Missing or bad key
        }

        public static Point Bury(string key)
        {
            if (key.Length == 0) return Null(); 

            Neo.SmartContract.Framework.Services.Neo.StorageContext ctx = Neo.SmartContract.Framework.Services.Neo.Storage.CurrentContext;
            string _skeyTag = key + _classKeyTag;

            Point p;
            byte[] bsta = Neo.SmartContract.Framework.Services.Neo.Storage.Get(ctx, _skeyTag + _sSTA);
            NeoTrace.Trace("Bury(skey).bs", bsta.Length, bsta);
            if (bsta.Length == 0)
            {
                p = Point.Missing();
            }
            else // not MISSING - bury it
            {
                p = Point.Tombstone(); // but don't overwrite existing field values - just tombstone it;s
                Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, _skeyTag + _sSTA, p._state.AsBigInteger());
                //Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, _skeyTag + _sX, p._x);
                //Neo.SmartContract.Framework.Services.Neo.Storage.Put(ctx, _skeyTag + _sY, p._y);
            }
            LogExt("Bury(skey).p", p);
            return p; // return Point p to signal if key is Missing or bad key
        }
    }
}

