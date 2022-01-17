using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class ClientStateC : NetworkState
    {
        public ClientStateC(SocketClient client) :base(client){ }

        public async override Task ExecuteStuff()
        {
            await Client.SendUDP(new HeartBeatRequestMessage()
            {
                Id = "UDPMessage",
                POSData = new POSData { Id = $"POS{Client.POSID}" }
            });
            await Client.SendTCP(new HeartBeatRequestMessage()
            {
                Id = "TCPMessage",
                POSData = new POSData { Id = $"POS{Client.POSID}" }
            });
            await Task.Delay(5000);
        }

        public override string ToString() => "ClientStateC";
    }
}
