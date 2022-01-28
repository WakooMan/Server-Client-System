using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class TestClient : ObjectMessageClient
    {
        public TestClient(string IPAddr, int _port) : base(IPAddr, _port) { ClientStateManager = new TestClientStateManager(this, new ClientStateC(this)); Bind<MessageHandler>(); }

        protected override void ClientLoop()
        {
            while (IsConnected)
            {
                ClientStateManager.CurrState.ExecuteStuff();
            }
        }
    }
}
