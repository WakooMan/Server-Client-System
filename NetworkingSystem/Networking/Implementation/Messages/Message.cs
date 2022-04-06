using ProtoBuf;
using System.Reflection;
using System;
using ProtoBuf.Meta;
using System.Collections.Generic;
namespace Networking.Implementation.Messages
{
    public enum MessageType
    {
        Response,Request,Broadcast
    }

    public enum Status
    {
        Success,Failure
    }
    [ProtoContract]
    public abstract class Message
    {
        private static MetaType BaseType = RuntimeTypeModel.Default[typeof(Message)];
        private static int MINID = 4;
        private static Dictionary<Type, int> SubClassIDs = new Dictionary<Type, int>();
        private static void AddType(Type t)
        {
            int ID = MINID++;
            BaseType.AddSubType(ID,t);
            SubClassIDs.Add(t, ID);
        }
        public static void AddMessageTypes(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(Message)))
                    AddType(type);
            }
        }
        public int MessageId { get; private set; }
        public MessageType Type { get; private set; }
        public EDeliveryMethod EMethod { get; private set; }

        protected Message(MessageType type, EDeliveryMethod eMethod) 
        {
            MessageId = SubClassIDs[GetType()];
            Type = type;
            EMethod = eMethod;
        }

        public delegate void ServerToClientMessageHandlerDelegate(Message message);
        public delegate void ClientToServerMessageHandlerDelegate(INetworkChannel peer, Message message);
    }
    [ProtoContract]
    public class Result
    {
        [ProtoMember(1)]
        public Status Status { get; set; }
        public Result() { }
        public Result(Status status) { Status = status; }
    }
}
