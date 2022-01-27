using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace Networking{
    [XmlRoot("Message")]
    [Serializable]
    public class KeepAliveResponseMessage : ManuallySerializedMessage
    {
        [XmlElement("Result")]
        [JsonProperty("result")]
        public Result Result { get; set; }

        public KeepAliveResponseMessage(Result result) : base(1,MessageType.Response,EDeliveryMethod.Reliable)
        {
            Result = result;
        }

        public KeepAliveResponseMessage():base(1, MessageType.Response, EDeliveryMethod.Reliable)
        { 
        }

        public override byte[] Serialize()
        {
            base.Serialize();
            writer.Write((int)Result.Status);
            return stream.ToArray();
        }
        public override void Deserialize(byte[] bytes)
        {
            base.Deserialize(bytes);
            Result = new Result((Status)BitConverter.ToInt32(bytes,12));
        }

        public override string ToString()
        {
            return "KeepAliveResponseMessage";
        }
    }
}
