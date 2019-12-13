using System;
using System.Collections.Generic;
using System.Text;

namespace FlashPayCrawler.IO
{
    public enum TrackState : byte
    {
        None,
        Added,
        Changed,
        Deleted
    }
}
