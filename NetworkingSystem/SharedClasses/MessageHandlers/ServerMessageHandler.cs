using System;
using System.Threading.Tasks;

namespace Networking
{
    public  class ServerMessageHandler<TStateEnum>
        where TStateEnum : Enum
    {
        public static SocketServer<TStateEnum> Server { get; set; }

        [XPathRoute("/Message[@type='Request' and @action='HeartBeat']")]
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
        public static async Task<BroadcastResponseMessage> HandleMessage(INetworkChannel Channel,BroadcastRequestMessage request)
        {
            Received(request);
            await Server.Broadcast(Channel.Id,request.BroadcastMessage).ConfigureAwait(false);
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
