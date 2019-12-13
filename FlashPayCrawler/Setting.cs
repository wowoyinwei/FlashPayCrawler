using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlashPayCrawler
{
    public class Setting
    {
        public static Setting Ins
        {
            get
            {
                if (ins == null)
                    ins = new Setting();
                return ins;
            }
        }

        private static Setting ins;

        public int Port;
        public string SimpleDbPath;
        public string EthCliUrl;
        public uint StartBlockNumber;

        public Setting()
        {
            JObject json = JObject.Parse(System.IO.File.ReadAllText("config.json"));
            Port = (int)json["port"];
            SimpleDbPath = (string)json["simpleDbPath"];
            EthCliUrl = (string)json["ethCliUrl"];
            StartBlockNumber = (uint)json["StartBlockNumber"];
        }

    }
}
