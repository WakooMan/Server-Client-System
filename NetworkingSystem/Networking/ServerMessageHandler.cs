using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public abstract class ServerMessageHandler
    {
        public static IServer Server { get; set; }

        [ObjectMessageRoute("KeepAliveRequestMessage")]
        public static KeepAliveResponseMessage HandleMessage(INetworkChannel Channel, KeepAliveRequestMessage request)
        {
            Received(request);
            var response = new KeepAliveResponseMessage(new Result(Status.Success));
            Sending(response);
            return response;
        }

        private static void Received<T>(T m) where T : Message => Console.WriteLine($"Received {typeof(T).Name}: ID [ {m.MessageId} ] ");
        private static void Sending<T>(T m) where T : Message => Console.WriteLine($"Sending {typeof(T).Name}: ID [ {m.MessageId} ] ");
    }
}
