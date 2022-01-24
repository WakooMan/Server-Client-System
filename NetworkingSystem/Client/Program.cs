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
using System.Threading;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Press Enter to Connect");
            Console.ReadLine();

            TestClient Client = new TestClient("127.0.0.1",9000);
            while (!Client.IsConnected) { Client.Update(); Thread.Sleep(100); }
            Client.StartClientLoopOnNewThread();
            while (Client.IsConnected)
            {
                Client.Update(); Thread.Sleep(100);
            }
        }
    }
}
