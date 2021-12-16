using Newtonsoft.Json.Linq;
using SharedClasses;
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
        static int POSID = System.Diagnostics.Process.GetCurrentProcess().Id;

        //static readonly XmlClientChannel Channel = new XmlClientChannel();
        static readonly JsonClientChannel Channel = new JsonClientChannel();
        static async Task Main(string[] args)
        {

            Console.WriteLine("Press Enter to Connect");
            Console.ReadLine();

            var MessageDispatcher = new JsonMessageDispatcher();
            //var MessageDispatcher = new XDocumentMessageDispatcher();

            MessageDispatcher.Bind<MessageHandler>();

            try
            {
                var EndPoint = new IPEndPoint(IPAddress.Loopback, 9000);
                MessageDispatcher.Bind(Channel);
                await Channel.ConnectAsync(EndPoint).ConfigureAwait(false);

                Console.WriteLine("Client Running");
                _ = Task.Run(() => HBLoop(-1));
            }
            catch (Exception ex)
            { Console.WriteLine($"Client Exception => {ex}"); }
            Console.ReadLine();

        }
        static async Task HBLoop(int count)
        {
            bool LoopControl() => count == -1 ? true : count-- > 0;
            while (LoopControl())
            {
                var hbRequest = new HeartBeatRequestMessage
                {
                    Id ="<3<3<3HB<3<3<3",
                    POSData = new POSData { Id = $"POS{POSID}" }
                };
                await Channel.SendAsync(hbRequest).ConfigureAwait(false);
                await Task.Delay(10*1000);
            }
        }
    }
}
