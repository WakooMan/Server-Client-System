
using Networking.Protobuf;
using System;

namespace Networking
{
    public abstract class ClientMessageHandler
    {
        public static IClient Client { get; set; }

        [ProtobufMsgRoute("KeepAliveResponseMessage")]
        public static void HandleMessage(KeepAliveResponseMessage response)
        {
            Console.WriteLine($"Received KeepAliveResponseMessage: {response?.Result?.Status}, PacketID:{response?.MessageId}");
        }
    }
}
