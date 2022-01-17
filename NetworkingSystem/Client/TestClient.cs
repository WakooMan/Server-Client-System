using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class TestClient : SocketClient
    {
        public TestClient(IPAddress IPAddr, int _port) : base(IPAddr, _port) { ClientStateManager = new TestClientStateManager(new ClientStateC(this),this); Bind<MessageHandler>(); }

        public async override Task ClientLoop()
        {
            while (true)
            {
                await ClientStateManager.CurrState.ExecuteStuff();
            }
        }
    }
}
