using FlashPayCrawler.libs;
using System.Threading.Tasks;
using FlashPayCrawler.Persistence;

namespace FlashPayCrawler.Crawlers
{
    public class CrawlerManager
    {
        public TransactionCrawler tranCrawler;
        public CrawlerManager()
        {
            tranCrawler = new TransactionCrawler();

            Task task = new Task(() =>{
                Start();
            });
            task.Start();
        }

        public void Start()
        {
            //获取爬虫处理到的高度
            uint curHeight = Singleton.Store.GetBlockNumber().TryGet(0);
            curHeight = curHeight == 0 ? Setting.Ins.StartBlockNumber : curHeight;
            Logger.LogCommon(string.Format("本地的高度{0}", curHeight));
            while (true)
            {
                try
                {
                    uint handlerHeight = curHeight + 1;
                    //看看这个高度有没有出块
                    bool isBlockExist = Singleton.EthHelper.IsBolckExist(handlerHeight);
                    if (!isBlockExist)
                    {
                        Logger.LogCommon(string.Format("高度{0}还没有出块",handlerHeight));
                        Task.Delay(1000);
                        return;
                    }
                    using (Snapshot snapshot = Singleton.Store.GetSnapshot())
                    {
                        Logger.LogCommon(string.Format("开始处理{0}高度的交易", handlerHeight));
                        tranCrawler.ProcessTxByBlockNumber(handlerHeight, snapshot);
                        Logger.LogCommon(string.Format("处理完成{0}高度的交易", handlerHeight));
                        //存储已经处理到的高度
                        snapshot.BlockNumber.Add(0, handlerHeight);
                        snapshot.Commit();
                    }
                    curHeight++;
                }
                catch (System.Exception e)
                {
                    Logger.LogCommon(e);
                }

            }



        }
    }
}
