using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class TestClient : SocketClient<ClientStates>
    {
        public TestClient(IPAddress IPAddr, int _port) : base(IPAddr, _port) { ClientState = new ClientStateC(0); }

        public async override void ClientLoop()
        {
            while (true)
            {
                var hbRequest = new HeartBeatRequestMessage
                {
                    Id = "<3<3<3HB<3<3<3",
                    POSData = new POSData { Id = $"POS{POSID}" }
                };

                var bcRequest = new BroadcastRequestMessage(new BroadcastMessage() { Id="Broadcast Message", POSData = new POSData { Id = $"POS{POSID}" } });
                bcRequest.Id = "Broadcast Request";
                bcRequest.POSData = new POSData { Id = $"POS{POSID}" };
                
                await Channel.SendAsync(hbRequest).ConfigureAwait(false);
                await Channel.SendAsync(bcRequest).ConfigureAwait(false);
                await Task.Delay(10 * 1000);
            }
        }
    }
}
