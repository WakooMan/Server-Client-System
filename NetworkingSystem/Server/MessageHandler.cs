using Networking;
using System;
using System.Threading.Tasks;

namespace Server
{
    public class MessageHandler: ServerMessageHandler
    {
        [ObjectMessageRoute("HeartBeatRequestMessage")]
        public static Task<HeartBeatResponseMessage> HandleMessage(INetworkChannel Channel, HeartBeatRequestMessage request)
        {
            Received(request);
            var response = new HeartBeatResponseMessage
            {
                Id = request.Id,
                POSData = request.POSData,
                Result = new Result { Status = Status.Success }
            };
            Sending(response);
            return Task.FromResult(response);
        }

        private static void Received<T>(T m) where T : Message => Console.WriteLine($"Received {typeof(T).Name}: POS ID [ {m.POSData.Id} ], Action [ {m.Action} ], ID [ {m.Id} ] ");
        private static void Sending<T>(T m) where T : Message => Console.WriteLine($"Sending {typeof(T).Name}: POS ID [ {m.POSData.Id} ], Action [ {m.Action} ], ID [ {m.Id} ] ");
    }
}
