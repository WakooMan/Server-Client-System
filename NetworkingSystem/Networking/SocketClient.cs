using System;
using System.Net;
using System.Threading.Tasks;

namespace Networking
{
    public abstract class SocketClient<TStateEnum> where TStateEnum : Enum
    {
        protected int POSID;
        protected readonly XmlClientChannel Channel;
        protected readonly XDocumentMessageDispatcher MessageDispatcher;
        //protected readonly JsonClientChannel Channel;
        //protected readonly  JsonMessageDispatcher MessageDispatcher;

        protected NetworkState<TStateEnum> ClientState;
        protected IPAddress IP;
        protected int port;
        public bool IsConnected { get => !Channel.IsClosed; }
        public SocketClient(IPAddress IPAddr, int _port)
        {
            IP = IPAddr;
            port = _port;
            POSID = System.Diagnostics.Process.GetCurrentProcess().Id;
            Channel = new XmlClientChannel();
            MessageDispatcher = new XDocumentMessageDispatcher();
            //Channel = new JsonClientChannel();
            //MessageDispatcher = new JsonMessageDispatcher();
            MessageDispatcher.Bind<ClientMessageHandler>();
            try
            {
                var EndPoint = new IPEndPoint(IP, port);
                MessageDispatcher.Bind(Channel);
                Channel.ConnectAsync(EndPoint).ConfigureAwait(false);
                Console.WriteLine("Client Running");
            }
            catch (Exception ex)
            { Console.WriteLine($"Client Exception => {ex}"); }
        }

        public async Task StartAsyncLoop()
         => await Task.Run(ClientLoop);

        //This is the client loop,which has the logic about when and what to send to the server.
        public abstract void ClientLoop();

    }
}
