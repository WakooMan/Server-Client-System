using ProtoBuf;
using System;

namespace Networking{
    public enum MessageType
    {
        Response,Request,Broadcast
    }

    public enum Status
    {
        Success,Failure
    }
    [ProtoContract]
    [ProtoInclude(4, typeof(KeepAliveRequestMessage))]
    [ProtoInclude(5, typeof(KeepAliveResponseMessage))]
    public abstract class Message
    {
        [ProtoMember(1)]
        public int MessageId { get; protected set; }
        [ProtoMember(2)]
        public MessageType Type { get; set; }
        [ProtoMember(3)]
        public EDeliveryMethod EMethod { get; set; }

        public Message(int id, MessageType type, EDeliveryMethod eMethod) 
        {
            MessageId = id;
            Type = type;
            EMethod = eMethod;
        }
        public abstract override string ToString();
    }
    [Serializable]
    [ProtoContract]
    public class Result
    {
        [ProtoMember(1)]
        public Status Status { get; set; }
        public Result() { }
        public Result(Status status) { Status = status; }
    }
}
