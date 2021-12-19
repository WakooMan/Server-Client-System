using SharedClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public  class MessageHandler
    {

        [XPathRoute("/Message[@type='Request' and @action='HeartBeat']")]
        [JsonRoute("$.action","HeartBeat")]
        public static Task<HeartBeatResponseMessage> HandleMessage(INetworkChannel Channel,HeartBeatRequestMessage request)
        {
            Received(request);
            var response = new HeartBeatResponseMessage
            {
                Id= request.Id,
                POSData= request.POSData,
                Result = new Result { Status = Status.Success}
            };
            Sending(response);
            return Task.FromResult(response);
        }

        [XPathRoute("/Message[@type='Request' and @action='Broadcast']")]
        [JsonRoute("$.action", "Broadcast")]
        public static async Task<BroadcastResponseMessage> HandleMessage(INetworkChannel Channel,BroadcastRequestMessage request)
        {
            Received(request);
            var broadcastmessage = new BroadcastMessage(request);
            await Program.Server.Broadcast(Channel.Id,broadcastmessage).ConfigureAwait(false);
            var response = new BroadcastResponseMessage
            {
                Id = request.Id,
                POSData = request.POSData,
                Result = new Result { Status = Status.Success }
            };
            Sending(response);
            return await Task.FromResult(response);
        }

        private static void Received<T>(T m) where T : Message => Console.WriteLine($"Received {typeof(T).Name}: POS ID [ {m.POSData.Id} ], Action [ {m.Action} ], ID [ {m.Id} ] ");
        private static void Sending<T>(T m) where T : Message => Console.WriteLine($"Sending {typeof(T).Name}: POS ID [ {m.POSData.Id} ], Action [ {m.Action} ], ID [ {m.Id} ] ");
    }
}
