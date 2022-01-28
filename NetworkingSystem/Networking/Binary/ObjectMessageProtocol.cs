using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Networking
{
    public class ObjectMessageProtocol : Protocol<object>
    {
        protected override object Decode(byte[] Message,bool IsUnreliable)
        {
            if (IsUnreliable) 
            {
                int messageId = BitConverter.ToInt32(Message, 0);
                UnreliableMessage message = (UnreliableMessage)Activator.CreateInstance(ObjectMessageDispatcher.RegisteredMessageTypes[messageId]);
                message.Deserialize(Message);
                return message;
            }
            else
            {
                var memstream = new MemoryStream();
                memstream.Write(Message, 0, Message.Length);
                memstream.Position = 0;
                return new BinaryFormatter().Deserialize(memstream);
            }
        }

        protected override byte[] EncodeBody<T>(T message)
        {
            if (message as ReliableMessage!=null)
            {
                var memstream = new MemoryStream();
                new BinaryFormatter().Serialize(memstream, message);
                return memstream.ToArray();
            }
            else if (message as UnreliableMessage!=null)
            {
                return (message as UnreliableMessage).Serialize();
            }
            return null;
        }
    }
}
