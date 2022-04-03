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
        public void Register<T>(Message.ServerToClientMessageHandlerDelegate<T> handler) where T : Message, new()
        {
            FromServerToClientMessageHandlers.Add(((T)Activator.CreateInstance(typeof(T))).MessageId, handler);
        }

        public void Register<T>(Message.ClientToServerMessageHandlerDelegate<T> handler) where T : Message, new()
        {
            FromClientToServerMessageHandlers.Add(((T)Activator.CreateInstance(typeof(T))).MessageId, handler); 
        }

        public void UnRegister<T>(Message.ServerToClientMessageHandlerDelegate<T> handler) where T : Message, new()
        {
            FromServerToClientMessageHandlers.Remove(((T)Activator.CreateInstance(typeof(T))).MessageId); 
        }

        public void UnRegister<T>(Message.ClientToServerMessageHandlerDelegate<T> handler) where T : Message, new()
        { 
            FromClientToServerMessageHandlers.Remove(((T)Activator.CreateInstance(typeof(T))).MessageId); 
        }
        #endregion
    }
}
