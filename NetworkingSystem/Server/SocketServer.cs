using SharedClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class SocketServer
    {
        readonly XDocumentMessageDispatcher messageDispatcher = new XDocumentMessageDispatcher();
        //readonly JsonMessageDispatcher messageDispatcher = new JsonMessageDispatcher();

        public SocketServer()
        {
            messageDispatcher.Bind<MessageHandler>();
        }

        public void Start(int port = 9000)
        {
            var EndPoint = new IPEndPoint(IPAddress.Loopback,port);

            var socket = new Socket(EndPoint.AddressFamily,SocketType.Stream,ProtocolType.Tcp);

            socket.Bind(EndPoint);
            socket.Listen(128);

            _= Task.Run(() => DoEcho(socket));
        }

        private async Task DoEcho(Socket socket)
        {
            do 
            {
                var clientsocket = await Task.Factory.FromAsync(
                    new Func<AsyncCallback,object,IAsyncResult>(socket.BeginAccept),
                    new Func<IAsyncResult,Socket>(socket.EndAccept),
                    null).ConfigureAwait(false);
                Console.WriteLine("ECHO SERVER :: CLIENT CONNECTED");

                var channel = new XmlChannel();
                //var channel = new JsonChannel();
                messageDispatcher.Bind(channel);

                channel.Attach(clientsocket);
                while (true) { }

            }while (true);
        }
    }
}
