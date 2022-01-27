
using System;

namespace Networking
{
    public abstract class ClientMessageHandler
    {
        public static IClient Client { get; set; }

        [ManualSerializationRoute("KeepAliveResponseMessage")]
        public static void HandleMessage(KeepAliveResponseMessage response)
        {
            Console.WriteLine($"Received KeepAliveResponseMessage: {response?.Result?.Status}, PacketID:{response?.MessageId}");
        }
    }
}
