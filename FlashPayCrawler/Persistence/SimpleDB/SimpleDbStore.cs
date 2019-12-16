using System;
using FlashPayCrawler.IO;
using NEL.SimpleDB;
using FlashPayCrawler.libs;

namespace FlashPayCrawler.Persistence.SimpleDB
{
    public class SimpleDbStore : Store, IDisposable
    {
        public DB db;

        public SimpleDbStore(string path)
        {
            db = new DB();
            string fullpath = System.IO.Path.GetFullPath(path);
            if (System.IO.Directory.Exists(fullpath) == false)
                System.IO.Directory.CreateDirectory(fullpath);
            string pathDB = System.IO.Path.Combine(fullpath, "crawler");
            try
            {
                db.Open(pathDB,true);
                Console.WriteLine("db opened in:" + pathDB);
            }
            catch (Exception err)
            {
                Console.WriteLine("error msg:" + err.Message);
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public override Cache<UInt32Wrapper, UInt32Wrapper> GetBlockNumber()
        {
            return new SimpleDbCache<UInt32Wrapper, UInt32Wrapper>(db,null,TableId.DATA_BlockNumber);
        }

        public override Cache<UInt160, UInt160> GetCareAddrs()
        {
            return new SimpleDbCache<UInt160, UInt160>(db, null, TableId.DATA_CareAddr);
        }

        public override Cache<UInt160, UInt160> GetCareAssets()
        {
            return new SimpleDbCache<UInt160, UInt160>(db, null, TableId.DATA_CareAsset);
        }

        public override Cache<UInt256, CareEvent> GetCareEvents()
        {
            return new SimpleDbCache<UInt256, CareEvent>(db, null, TableId.DATA_CareEvent);
        }

        public override Cache<TransferKey, TransferGroup> GetTransferGroup()
        {
            return new SimpleDbCache<TransferKey, TransferGroup>(db,null,TableId.DATA_Transfer);
        }

        public override Cache<UInt160, TransferBlockNumberList> GetTransferBlockNumberList()
        {
            return new SimpleDbCache<UInt160, TransferBlockNumberList>(db, null, TableId.DATA_TransferBlockNumberList);
        }

        public override Snapshot GetSnapshot()
        {
            return new SimpleDbSnapShot(db);
        }

        public override byte[] Get(byte[] tableId, byte[] key)
        {
            return db.GetDirect(tableId,key);
        }
    }
}
