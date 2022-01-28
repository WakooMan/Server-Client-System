using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Messages
{
    [Serializable]
    public class RandomMessage : UnreliableMessage
    {
        public byte[] Data { get; set; }
        public RandomMessage() : base(2, MessageType.Broadcast)
        {
            Data = null;
        }
        public RandomMessage(byte[] data) : base(2, MessageType.Broadcast)
        {
            Data = data;
        }

        public override void Deserialize(byte[] bytes)
        {
            base.Deserialize(bytes);
            Data = new byte[bytes.Length - 12]; 
            Array.Copy(bytes,12,Data,0,bytes.Length-12);
        }

        public override byte[] Serialize()
        {
            base.Serialize();
            writer.Write(Data);
            return stream.ToArray();
        }

        public override string ToString()
        {
            return "RandomMessage";
        }
    }
}
