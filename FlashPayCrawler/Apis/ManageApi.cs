using FlashPayCrawler.IO;
using FlashPayCrawler.libs;
using FlashPayCrawler.RPC;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FlashPayCrawler.Apis
{
    public class ManageApi : BaseApi
    {
        public ManageApi(string node) : base(node)
        {
        }


        protected override JArray ProcessRes(JsonRPCrequest req)
        {
            JArray result = new JArray();
            result = base.ProcessRes(req);

            switch (req.method)
            {
                case "addCareAddr":
                    {
                        using (var snapshot = Singleton.Store.GetSnapshot())
                        {
                            snapshot.CareAddrs.Add(new UInt160((string)req.@params[0]), new UInt160((string)req.@params[0]));
                            snapshot.Commit();
                        }
                        //Singleton.CareAddrs.Add(new UInt160((string)req.@params[0]), new UInt160((string)req.@params[0]));
                        result = getJAbyKV("result", true);
                        break;
                    }
                case "existCareAddr":
                    {
                        UInt160 key = new UInt160((string)req.@params[0]);
                        UInt160 value = Singleton.Store.GetCareAddrs().TryGet(key);
                        //UInt160 value = Singleton.CareAddrs.TryGet(key);
                        result = result = getJAbyKV("result", key.Equals(value));
                        break;
                    }
                case "addCareAsset":
                    {
                        using (var snapshot = Singleton.Store.GetSnapshot())
                        {
                            snapshot.CareAssets.Add(new UInt160((string)req.@params[0]), new UInt160((string)req.@params[0]));
                            snapshot.Commit();
                        }
                        //Singleton.CareAssets.Add(new UInt160((string)req.@params[0]), new UInt160((string)req.@params[0]));
                        result = getJAbyKV("result", true);
                        break;
                    }
                case "existCareAsset":
                    {
                        UInt160 key = new UInt160((string)req.@params[0]);
                        UInt160 value = Singleton.Store.GetCareAssets().TryGet(key);
                        //UInt160 value = Singleton.CareAssets.TryGet(key);
                        result = result = getJAbyKV("result", key.Equals(value));
                        break;
                    }
                case "addCareEvent":
                    {
                        string eventStr = (string)req.@params[0];
                        string hexStr = Conversion.Bytes2HexString(Encoding.UTF8.GetBytes(eventStr));
                        UInt256 hash = Singleton.EthHelper.GetSha3(hexStr);
                        CareEvent careEvent = new CareEvent() { Hash = hash, EventStr = eventStr, HexStr = hexStr };
                        using (var snapshot = Singleton.Store.GetSnapshot())
                        {
                            snapshot.CareEvents.Add(hash, careEvent);
                            snapshot.Commit();
                        }
                        //Singleton.CareEvents.Add(hash, careEvent);
                        result = getJAbyKV("result", true);
                        break;
                    }
                case "getCareEvents":
                    {
                        JArray j = new JArray();
                        j.Add(Singleton.Store.GetCareEvents().Find().Select(p=>p.Value.ToJson()));
                        result = j;
                        break;
                    }

            }
            return result;
        }
    }
}
