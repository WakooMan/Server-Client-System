using Networking;
using Networking.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class TestClient : Client<Message>
    {
        public TestClient(string IPAddr, int _port) : base(IPAddr, _port) { ClientStateManager = new TestClientStateManager(this, new ClientStateC(this)); Bind<MessageHandler>(); }

        protected override void ClientLoop()
        {
            while (IsConnected)
            {
                ClientStateManager.CurrState.ExecuteStuff();
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
                ClientStateManager.CurrState.ExecuteStuff();
            }
        }
    }
}
