using NEL.SimpleDB;
using FlashPayCrawler.IO;
using FlashPayCrawler.libs;

namespace FlashPayCrawler.Persistence.SimpleDB
{
    public class SimpleDbSnapShot : Snapshot
    {
        private readonly DB db;
        private readonly ISnapShot snapshot;
        private readonly IWriteBatch batch;

        public override Cache<LogKey, Log> Logs { get; }

        public override Cache<UInt160, UInt160> CareAddrs { get; }

        public override Cache<UInt160, UInt160> CareAssets { get; }

        public override Cache<UInt256, CareEvent> CareEvents { get; }

        public override Cache<TransferKey, TransferGroup> Transfers { get; }

        public override Cache<UInt32Wrapper, UInt32Wrapper> BlockNumber { get; }

        public SimpleDbSnapShot(DB db)
        {
            this.db = db;
            this.snapshot = db.UseSnapShot();
            this.batch = db.CreateWriteBatch();
            Logs = new SimpleDbCache<LogKey, Log>(this.db,this.batch,TableId.DATA_Log);
            CareAddrs = new SimpleDbCache<UInt160, UInt160>(this.db,this.batch,TableId.DATA_CareAddr);
            CareAssets = new SimpleDbCache<UInt160,UInt160>(this.db,this.batch,TableId.DATA_CareAsset);
            CareEvents = new SimpleDbCache<UInt256, CareEvent>(this.db,this.batch,TableId.DATA_CareEvent);
            Transfers = new SimpleDbCache<TransferKey, TransferGroup>(this.db,this.batch,TableId.DATA_Transfer);
            BlockNumber = new SimpleDbCache<UInt32Wrapper, UInt32Wrapper>(this.db,this.batch,TableId.DATA_BlockNumber);
        }

        public override void Commit()
        {
            base.Commit();
            db.WriteBatch(batch);
        }

        public override void Dispose()
        {
            snapshot.Dispose();
        }
    }
}
