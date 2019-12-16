using FlashPayCrawler.libs;
using FlashPayCrawler.RPC;
using FlashPayCrawler.IO;
using Newtonsoft.Json.Linq;

namespace FlashPayCrawler.Apis
{
    public class OriginalApi : BaseApi
    {
        public OriginalApi(string node) : base(node)
        {
            
        }

        protected override JArray ProcessRes(JsonRPCrequest req)
        {
            JArray result = new JArray();
            result = base.ProcessRes(req);

            switch (req.method)
            {
                case "getTransferByAddressAndBlockNumber":
                    {
                        UInt160 address = new UInt160((string)req.@params[0]);
                        uint blockNumber =uint.Parse(req.@params[1].ToString());
                        TransferGroup trans = Singleton.Store.GetTransferGroup().TryGet(new TransferKey() { address = address,blockNumber = blockNumber});
                        JArray ja = new JArray();
                        for (var i = 0; i < trans.transfers.Length; i++)
                        {
                            ja.Add(trans.transfers[i].ToJson());
                        }
                        result = ja;
                        break;
                    }
                case "getAllTransferByAddress":
                    {
                        UInt160 address = new UInt160((string)req.@params[0]);
                        TransferBlockNumberList list = Singleton.Store.GetTransferBlockNumberList().TryGet(address);
                        JArray ja = new JArray();
                        for (var i = 0; i < list.blockNumberList.Count; i++)
                        {
                            uint blockNumber = list.blockNumberList[i];
                            TransferGroup trans = Singleton.Store.GetTransferGroup().TryGet(new TransferKey() { address = address, blockNumber = blockNumber });
                            for (var ii = 0; ii < trans.transfers.Length; ii++)
                            {
                                ja.Add(trans.transfers[ii].ToJson());
                            }
                        }
                        result = ja;
                        break;
                    }
            }
            return result;
        }
    }
}
