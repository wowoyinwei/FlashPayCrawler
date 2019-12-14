using FlashPayCrawler.IO;
using FlashPayCrawler.libs;
using FlashPayCrawler.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace FlashPayCrawler.Crawlers
{
    public class TransactionCrawler
    {
        public Dictionary<UInt160, List<Transfer>> dic = new Dictionary<UInt160, List<Transfer>>();

        public TransactionCrawler()
        {
        }

        public void ProcessTxByBlockNumber(uint blockNumber, Snapshot snapshot)
        {
            dic.Clear();
            uint txCount = Singleton.EthHelper.GetBlockTransactionCountByNumber(blockNumber);
            for (uint i = 0; i < txCount; i++)
            {
                UInt256 txHash = Singleton.EthHelper.GetTransactionHashByBlockNumberAndIndex(blockNumber, i);
                Logger.LogCommon(txHash);
                ProcessLogsByTxHash(txHash, snapshot);
            }

            /////
            foreach (var v in dic.Keys)
            {
                snapshot.Transfers.Add(
                    new TransferKey() { address = v, blockNumber = blockNumber},
                    new TransferGroup() { transfers = dic[v].ToArray()}
                    );
            }
        }

        public void ProcessLogsByTxHash(UInt256 txHash, Snapshot snapshot)
        {
            List<Log> logs = Singleton.EthHelper.GetTransactionReceiptLogs(txHash);
            for (var i = 0; i < logs.Count; i++)
            {
                Log l = logs[i];
                LogKey lk = new LogKey() { LogIndex = l.LogIndex, TransactionHash = l.TransactionHash };
                if (l.Topics.Length == 0)
                    continue;
                UInt256 eventHash = new UInt256(l.Topics[0]);
                //如果这个通知是需要在意的合约和通知 就再处理一波
                CareEvent ce = snapshot.CareEvents.TryGet(eventHash);
                UInt160 asset = snapshot.CareAssets.TryGet(l.ContractAddress);
                if (ce != null && asset != null)
                {
                    l.Event = ce.EventStr;
                    snapshot.Logs.Add(lk, l);
                    //如果是transfer通知那就再额外处理一波
                    if (ce.EventStr == "Transfer(address,address,uint256)")
                    {
                        ProcessTransferLog(l);
                    }
                }
            }
        }

        public void ProcessTransferLog(Log l)
        {
            Transfer t = new Transfer();
            t.Asset = l.ContractAddress;
            t.LogIndex = l.LogIndex;
            t.From = new UInt160(l.Topics[1].Substring(26));
            t.To = new UInt160(l.Topics[2].Substring(26));
            var s = l.Data[0];
            Logger.LogCommon(s);
            t.Value = new BigInteger(l.Data[0].HexString2Bytes().Reverse().ToArray()).ToString();
            t.TransactionHash = l.TransactionHash;

            //处理from
            if (dic.ContainsKey(t.From))
            {
                dic[t.From].Add(t);
            }
            else
            {
                dic[t.From] = new List<Transfer>();
                dic[t.From].Add(t);
            }

            //处理to
            //处理from
            if (dic.ContainsKey(t.To))
            {
                dic[t.To].Add(t);
            }
            else
            {
                dic[t.To] = new List<Transfer>();
                dic[t.To].Add(t);
            }
        }
    }
}
