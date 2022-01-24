
using System;

namespace Networking
{
    public abstract class ClientMessageHandler
    {
        public static Client Client { get; set; }

        [ObjectMessageRoute("KeepAliveResponseMessage")]
        public static void HandleMessage(KeepAliveResponseMessage response)
        {
            Console.WriteLine($"Received KeepAliveResponseMessage: {response?.Result?.Status}, PacketID:{response?.Id}");
        }
    }
}
