using System;
using FlashPayCrawler.IO;
using FlashPayCrawler.libs;

namespace FlashPayCrawler.Persistence
{
    public abstract class Snapshot : IDisposable
    {
        public abstract Cache<LogKey, Log> Logs { get; }
        public abstract Cache<UInt160, UInt160> CareAddrs { get; }
        public abstract Cache<UInt160, UInt160> CareAssets { get; }
        public abstract Cache<UInt256, CareEvent> CareEvents { get; }
        public abstract Cache<TransferKey,TransferGroup> Transfers { get; }
        public abstract Cache<UInt32Wrapper, UInt32Wrapper> BlockNumber { get; }

        public virtual void Commit()
        {
            Logs?.Commit();
            CareAddrs?.Commit();
            CareAssets?.Commit();
            CareEvents?.Commit();
            Transfers?.Commit();
            BlockNumber?.Commit();
        }

        public virtual void Dispose()
        {
        }
    }
}
