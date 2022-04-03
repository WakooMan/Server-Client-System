using Networking.Implementation.Messages;
using Networking.Implementation.Messages.FromClientMessages;
using System;
using System.Threading;

namespace Networking.Implementation
{
    public class Client : ClientBase<Message>
    {
        public static Client Current { get; private set; } = null;
        private Client(Action<Client> OnConnectedEvent) : base(Dispatch)
        {
            AddOnConnectedEvent(
                () =>
                {
                    OnConnectedEvent?.Invoke(this);
                });
            AddOnConnectedEvent(() => { Send(new NetworkDataRequestMessage(Network.Data.MainPlayer)); });
            Network.Register(new BasicNetworkStateClientComponent());
        }
        public static void Init(Action<Client> OnConnectedEvent)
        {
            if (Current == null && !Network.IsServer)
            { 
                Current = new Client(OnConnectedEvent); 
            }
        }

        public static void StartClient(string IPAddr, int _port)
        {
            if (Current != null)
            {
                Current.ConnectTo(IPAddr, _port);
            }
        }

        public static void StartClientOnNewThread(string IPAddr, int _port)
        {
            if (Current!=null)
            {
                new Thread(() =>
                {
                    Current.ConnectTo(IPAddr, _port);
                    while (Current!=null)
                    {
                        Current.Update();
                    }
                }).Start();
            }
        }

        public static void StopClient() 
        {
            if (Current!=null)
            {
                Current.Disconnect();
                Current = null;
            }
        }

        public override void Update()
        {
            base.Update();
            if (IsConnected)
            {
                if (DateTime.Compare(DateTime.UtcNow.Subtract(new TimeSpan(0, 0, 5)), LastSentKeepAliveMsg) > 0)
                {
                    Send(new KeepAliveRequestMessage());
                    LastSentKeepAliveMsg = DateTime.UtcNow;
                }
            }
        }

        private static void Dispatch(Message msg)
        { 
            Network.InvokeFromServerMessageHandler(msg); 
        }

        public override void Send<T>(T msg)
        {
            Message message = msg as Message;
            if (message != null)
                Send(msg, message.EMethod);
            else
                throw new NotMessageException();
        }

        /*private static void RegisterInitialMessages()
        {
            BattleNetwork.Register(new Message.ServerMessageHandlerDelegate<NewGuidAddedMessage>(HandleNewGuidAddedMessage));
            BattleNetwork.Register(new Message.ServerMessageHandlerDelegate<AssignGuidMessage>(HandleAssignGuidMessage));
            BattleNetwork.Register(new Message.ServerMessageHandlerDelegate<KeepAliveResponseMessage>(HandleKeepAliveResponseMessage));
            BattleNetwork.Register(new Message.ServerMessageHandlerDelegate<BattleDataResponseMessage>(HandleBattleDataResponseMessage));
        }

        private static void HandleAssignGuidMessage(AssignGuidMessage message)
        {
            BattleData.InitBattleData(message.AssignedClientId);
        }

        private static void HandleNewGuidAddedMessage(NewGuidAddedMessage message)
        {
            BattleData.Current.AddPlayer(message.playerId, new Player(null));
        }

        private static void HandleBattleDataResponseMessage(BattleDataResponseMessage message)
        {
            MySubModule.BattleDataResponse = message;
        }*/
    }
}
