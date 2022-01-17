using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public class ObjectMessageProtocol : Protocol<object>
    {
        protected override object Decode(byte[] Message)
        {
            var memstream = new MemoryStream();
            memstream.Write(Message,0,Message.Length);
            memstream.Position = 0;
            return new BinaryFormatter().Deserialize(memstream);
        }

        protected override byte[] EncodeBody<T>(T message)
        {
            var memstream = new MemoryStream();
            new BinaryFormatter().Serialize(memstream,message);
            return memstream.ToArray();
        }
    }
}
