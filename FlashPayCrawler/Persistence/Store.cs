using FlashPayCrawler.IO;
using FlashPayCrawler.libs;

namespace FlashPayCrawler.Persistence
{
    public abstract class Store
    {
        public abstract Cache<UInt160, UInt160> GetCareAddrs();
        public abstract Cache<UInt160, UInt160> GetCareAssets();
        public abstract Cache<UInt256, CareEvent> GetCareEvents();
        public abstract Cache<UInt32Wrapper, UInt32Wrapper> GetBlockNumber();
        public abstract Cache<TransferKey, TransferGroup>GetTransferGroup();


        public abstract Snapshot GetSnapshot();
        public abstract byte[] Get(byte[] tableId,byte[] key);
    }
}
