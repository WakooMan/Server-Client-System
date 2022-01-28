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
    [Serializable]
    public abstract class Message
    {
        public int MessageId { get; protected set; }

        public MessageType Type { get; set; }

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
    public class Result
    {
        public Status Status { get; set; }

        public Result(Status status) { Status = status; }
    }
}
