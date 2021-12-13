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
        static readonly XmlClientChannel Channel = new XmlClientChannel();
        static readonly XDocumentMessageDispatcher MessageDispatcher = new XDocumentMessageDispatcher();
        static async Task Main(string[] args)
        {
            MessageDispatcher.Register<HeartBeatResponseMessage>(MessageHandler.HandleMessage);
            MessageDispatcher.Register<SubmitBasketResponseMessage>(MessageHandler.HandleMessage);

            Console.WriteLine("Press Enter to Connect");
            Console.ReadLine();

            var EndPoint = new IPEndPoint(IPAddress.Loopback,9000);

            

            Channel.OnMessage(MessageDispatcher.DispatchAsync);

            await Channel.ConnectAsync(EndPoint).ConfigureAwait(false);


            _ = Task.Run(() => HBLoop(10));

            var SubmitBasket = new SubmitBasketRequestMessage
            {
                Id = "BASKET_0001",
                POSData = new POSData { Id = "POS001" }
            };

            await Channel.SendAsync(SubmitBasket).ConfigureAwait(false);

            Console.ReadLine();

        }
        static async Task HBLoop(int interval)
        {
            int requestId = 1;
            while (true)
            {
                var hbRequest = new HeartBeatRequestMessage
                {
                    Id =$"<3<3<3{requestId}<3<3<3",
                    POSData = new POSData { Id = "POS001" }
                };
                await Channel.SendAsync(hbRequest).ConfigureAwait(false);
                await Task.Delay(interval*1000);
            }
        }
    }
}
