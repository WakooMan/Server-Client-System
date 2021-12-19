using Newtonsoft.Json.Linq;
using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            Console.WriteLine("Press Enter to Connect");
            Console.ReadLine();

            TestClient Client = new TestClient(IPAddress.Loopback,9000);
            await Client.StartAsyncLoop();
            while (Client.IsConnected)
            { }
        }
    }
}
