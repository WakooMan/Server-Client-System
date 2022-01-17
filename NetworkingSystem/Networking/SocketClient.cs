using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Networking
{
    public abstract class SocketClient
    {
        public int POSID;
        protected readonly ObjectMessageClientChannels Channels;
        protected readonly ObjectMessageDispatcher MessageDispatcher;
        public bool Waiting { get; set; }

        public NetworkStateManager ClientStateManager { get; protected set; }
        protected IPAddress IP;
        protected int port;
        public bool IsConnected { get => !Channels.IsClosed; }
        public SocketClient(IPAddress IPAddr, int _port)
        {
            Waiting = false;
            IP = IPAddr;
            port = _port;
            POSID = System.Diagnostics.Process.GetCurrentProcess().Id;
            Channels = new ObjectMessageClientChannels();
            MessageDispatcher = new ObjectMessageDispatcher();
            try
            {
                var EndPoint = new IPEndPoint(IP, port);
                MessageDispatcher.Bind(Channels.TCPChannel);
                MessageDispatcher.Bind(Channels.UDPChannel);
                Channels.ConnectAsync(EndPoint).ConfigureAwait(false);
                Console.WriteLine("Client Running");
            }
            catch (Exception ex)
            { Console.WriteLine($"Client Exception => {ex}"); }
        }

        public void Bind<TController>() => MessageDispatcher.Bind<TController>();

        public void Disconnect()
        { 
            if(!Channels.IsClosed)
                Channels.Close();
        }
        public async Task StartAsyncLoop()
         => await Task.Run(ClientLoop);

        //This is the client loop,which has the logic about when and what to send to the server.
        public abstract Task ClientLoop();

        public async Task SendTCP(Message msg)
        {
            await Channels.SendTCP(msg).ConfigureAwait(false);
        }

        public async Task SendUDP(Message msg)
        {
            await Channels.SendUDP(msg).ConfigureAwait(false);
        }
    }
}
