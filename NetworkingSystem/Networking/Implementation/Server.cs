using Networking.Implementation.Messages;
using System;
using System.Threading;
using Networking.Implementation.Messages.FromServerMessages;
namespace Networking.Implementation
{
    public class Server : ServerBase<Message>
    {
        public static Server Current { get; private set; } = null;
        private Server() : base(Dispatch)
        {
            AddOnClientDisconnectedEvent (OnClientDisconnectedEvent);
            Network.Register(new BasicNetworkStateServerComponent());
        }

        public static void StartServer(int maxclients,int port)
        {
            if (Current!=null)
            {
                Current.Start(port,maxclients);
                Network.Data.MainPlayer.ID = Guid.NewGuid();
                Network.Data.Add(Network.Data.MainPlayer);
            }
        }

        public static void Init()
        {
            if (!Network.IsClient && Current == null)
            { 
                Current = new Server(); 
            }
        }

        public static void StartServerOnNewThread(int maxclients,int port)
        {
            if (Current != null && !Current.Running)
            {
                new Thread(() =>
                {
                    Current.Start(port,maxclients);
                    Network.Data.MainPlayer.ID = Guid.NewGuid();
                    Network.Data.Add(Network.Data.MainPlayer);
                    while (Current!=null)
                    {
                        Current.Update();
                    }
                }).Start();
            }
        }

        public static void StopServer()
        {
            if (Current!=null)
            {
                Current.Stop();
                Current = null;
            }
        }

        public override void Broadcast<T>(Guid ExceptionId, T Message)
        {
            Message msg = Message as Message;
            if (msg != null)
                Broadcast(ExceptionId, Message, msg.EMethod);
            else
                throw new NotMessageException();
        }

        public override bool CanPlayerJoin()
        {
            return base.CanPlayerJoin();
        }

        public void OnClientDisconnectedEvent(INetworkChannel Channel)
        {
            Broadcast(Channel.Id, new PlayerDisconnectedMessage(Channel.Id));
            Network.Data.Remove(Channel.Id);
        }



        public override void SendTo<T>(Guid Id, T Message)
        {
            Message msg = Message as Message;
            if (msg != null)
                SendTo(Id, Message, msg.EMethod);
            else
                throw new NotMessageException();
        }

        public static void Dispatch(INetworkChannel Channel, Message msg)
        {
            Network.InvokeFromClientMessageHandler(Channel, msg); 
        }
    }
}
