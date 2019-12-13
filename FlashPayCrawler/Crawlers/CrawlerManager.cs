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
                    using (Snapshot snapshot = Singleton.Store.GetSnapshot())
                    {
                        uint handlerHeight = curHeight + 1;
                        Logger.LogCommon(string.Format("开始处理{0}高度的交易", handlerHeight));
                        tranCrawler.ProcessTxByBlockNumber(handlerHeight, snapshot);
                        Logger.LogCommon(string.Format("处理完成{0}高度的交易", handlerHeight));
                        curHeight = handlerHeight;
                        //存储已经处理到的高度
                        snapshot.BlockNumber.Add(0, handlerHeight);
                        snapshot.Commit();
                    }
                }
                catch (System.Exception e)
                {
                    curHeight--;
                    Logger.LogCommon(e);
                }

            }



        }
    }
}
