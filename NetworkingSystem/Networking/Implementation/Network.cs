using Networking.Implementation.Messages;
using System;
using System.Collections.Generic;
using System.Reflection;
namespace Networking.Implementation
{
    public class Network
    {
        #region Static Methods,Fields,Properties
        private static Network Current { get; set; } = null;
        private static Dictionary<int, Delegate> FromServerToClientMessageHandlers = null;
        private static Dictionary<int, Delegate> FromClientToServerMessageHandlers = null;
        public static NetworkData Data { get; private set; }
        public static bool IsClient { get => Client.Current != null; }
        public static bool IsServer { get => Server.Current != null; }
        public static Server Server { get => Server.Current; }
        public static Client Client { get => Client.Current; }
        public static void Init(string Name)
        {
            Current = new Network();
            Data = new NetworkData(Name);
            FromServerToClientMessageHandlers = new Dictionary<int, Delegate>();
            FromClientToServerMessageHandlers = new Dictionary<int, Delegate>();
            Message.AddMessageTypes(Assembly.GetExecutingAssembly());
        }
        public static void Delete()
        {
            Current = null;
            Data = null;
            ClearMessageHandlers();
            FromClientToServerMessageHandlers = null;
            FromServerToClientMessageHandlers = null;
        }
        public static void Register(BaseNetworkStateComponent component)
        {
            component.AddMessageHandlers(Current);
        }
        public static void UnRegister(BaseNetworkStateComponent component)
        {
            component.RemoveMessageHandlers(Current);
        }
        public static void InvokeFromServerMessageHandler(Message msg)
        {
            if (FromServerToClientMessageHandlers.ContainsKey(msg.MessageId))
                FromServerToClientMessageHandlers[msg.MessageId].DynamicInvoke(new object[] { msg });
        }
        public static void InvokeFromClientMessageHandler(INetworkChannel channel, Message msg)
        {
            if (FromClientToServerMessageHandlers.ContainsKey(msg.MessageId))
                FromClientToServerMessageHandlers[msg.MessageId].DynamicInvoke(new object[] { channel, msg });
        }
        public static void ClearFromClientToServerMessageHandlers()
        {
            FromClientToServerMessageHandlers.Clear();
        }
        public static void ClearFromServerToClientMessageHandlers()
        {
            FromServerToClientMessageHandlers.Clear();
        }
        public static void ClearMessageHandlers()
        {
            FromServerToClientMessageHandlers.Clear();
            FromClientToServerMessageHandlers.Clear();
        }
        #endregion
        #region Normal Methods,Fields,Properties
        public void Register(int MessageID, Message.ServerToClientMessageHandlerDelegate handler)
        {
            FromServerToClientMessageHandlers.Add(MessageID, handler);
        }

        public void Register(int MessageID,Message.ClientToServerMessageHandlerDelegate handler)
        {
            FromClientToServerMessageHandlers.Add(MessageID, handler); 
        }

        public void UnRegister(int MessageID,bool IsServerComponent)
        {
            if(IsServerComponent)
                FromClientToServerMessageHandlers.Remove(MessageID);
            else
                FromServerToClientMessageHandlers.Remove(MessageID); 
        }
        #endregion
    }
}
