using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public class ManualSerializationProtocol : Protocol<object>
    {
        protected override object Decode(byte[] Message)
        {
            int messageId = BitConverter.ToInt32(Message, 0);
            ManuallySerializedMessage message =(ManuallySerializedMessage)Activator.CreateInstance(ManualSerializationDispatcher.RegisteredMessageTypes[messageId]);
            message.Deserialize(Message);
            return message;
        }

        protected override byte[] EncodeBody<T>(T message)
        {
            return (message as ManuallySerializedMessage).Serialize();
        }
    }
}
