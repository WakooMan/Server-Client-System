using Networking;
using Networking.Messages;
using System;
using System.Threading.Tasks;

namespace Client
{
    public class MessageHandler : ClientMessageHandler
    {
        [ObjectMessageRoute("RandomMessage")]
        public static void HandleMessage(RandomMessage Message)
        {
            ReceivedMsg(Message);
        }
        private static void ReceivedMsg<T>(T m) where T : RandomMessage => Console.WriteLine($"Received {typeof(T).Name}: ID [ {m.MessageId} ] with length of bytes: {m.Data.Length} ");
    }
}