using Newtonsoft.Json.Linq;
using FlashPayCrawler.IO;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FlashPayCrawler.libs
{
    public class EthHelper
    {
        string url;

        public EthHelper(string url)
        {
            this.url = url;
        }

        public string PackPostDataStr(string method, JArray @params)
        {
            JObject postData = new JObject();
            postData.Add("jsonrpc", "2.0");
            postData.Add("method", method);
            postData.Add("params", @params);
            postData.Add("id", 1);
            return Newtonsoft.Json.JsonConvert.SerializeObject(postData);

        }

        public object ProcessResult(string result)
        {
            JObject json = JObject.Parse(result);
            if (json.ContainsKey("result"))
                return json["result"];
            else if (json.ContainsKey("error"))
                throw new FormatException((string)json["error"]);
            else
                throw new FormatException();

        }

        public bool IsBolckExist(uint blockNumber)
        {
            string postDataStr = PackPostDataStr("eth_blockNumber", new JArray() { });
            string result = HttpHelper.Post(url, postDataStr);
            uint curBlockNumber = (ProcessResult(result).ToString()).Replace("0x", "").HexToUint();
            return blockNumber<= curBlockNumber;
        }

        public uint GetBlockTransactionCountByNumber(uint blockNumber)
        {
            string postDataStr = PackPostDataStr("eth_getBlockTransactionCountByNumber", new JArray() { blockNumber.ToString("x").FormatHexStr() });
            string result = HttpHelper.Post(url, postDataStr);
            return Convert.ToUInt32(ProcessResult(result).ToString(),16);
        }

        public UInt256 GetTransactionHashByBlockNumberAndIndex(uint blockNumber, uint index)
        {
            string postDataStr = PackPostDataStr(
                "eth_getTransactionByBlockNumberAndIndex",
                new JArray() { blockNumber.ToString("x").FormatHexStr(), index.ToString("x").FormatHexStr() }
                );
            string result = HttpHelper.Post(url, postDataStr);
            return new UInt256(((JObject)ProcessResult(result))["hash"].ToString());
        }

        public List<Log> GetTransactionReceiptLogs(UInt256 txHash)
        {
            string postDataStr = PackPostDataStr(
                "eth_getTransactionReceipt",
                new JArray() { txHash.ToString() }
                );
            string result = HttpHelper.Post(url, postDataStr);
            JArray ja = (JArray)((JObject)ProcessResult(result))["logs"];
            List<Log> logs = new List<Log>();
            try
            {
                logs.AddRange(ja.Select(p => Log.FromJson((JObject)p)));
            }
            catch (Exception e)
            {
                Logger.LogCommon(e);
            }
            return logs;
        }

        public UInt256 GetSha3(string hexStr)
        {
            hexStr = hexStr.FormatHexStr();
            string postDataStr = PackPostDataStr("web3_sha3", new JArray() { hexStr });
            string result = HttpHelper.Post(url, postDataStr);
            return new UInt256(ProcessResult(result).ToString());
        }
    }
}
