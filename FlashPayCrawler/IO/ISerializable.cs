using Newtonsoft.Json.Linq;
using System.IO;

namespace FlashPayCrawler.IO
{
    public interface ISerializable
    {
        void Serialize(BinaryWriter writer);
        byte[] Serialize();
        void Deserialize(BinaryReader reader);
    }
}
