﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public abstract class ManuallySerializedMessage : Message
    {
        protected MemoryStream stream;
        protected BinaryWriter writer; 
        protected ManuallySerializedMessage(int id, MessageType type, EDeliveryMethod eMethod) : base(id, type, eMethod)
        {
        }

        public ManuallySerializedMessage() :base(0,0,0)
        {

        }

        public override abstract string ToString();
        //This should be called, when overriden
        public virtual byte[] Serialize()
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
            writer.Write(MessageId);
            writer.Write((int)Type);
            writer.Write((int)EMethod);
            return stream.ToArray();
        }
        public virtual void Deserialize(byte[] bytes)
        {
            MessageId = BitConverter.ToInt32(bytes, 0);
            Type= (MessageType)BitConverter.ToInt32(bytes,4);
            EMethod = (EDeliveryMethod)BitConverter.ToInt32(bytes, 8);
        }
    }
}