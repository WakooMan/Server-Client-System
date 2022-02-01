using System.IO;
using ProtoBuf;

namespace Networking.Protobuf
{
    public class ProtobufMsgProtocol<TMessageType> : Protocol<object>
    {
        protected override object Decode(byte[] Message)
        {
            var memstream = new MemoryStream(Message);
            return Serializer.Deserialize<TMessageType>(memstream);
        }

        protected override byte[] EncodeBody<T>(T message)
        {
            var memstream = new MemoryStream();
            Serializer.Serialize(memstream, message);
            return memstream.ToArray();
        }
    }
}
