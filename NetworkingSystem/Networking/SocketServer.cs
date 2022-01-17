
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Networking
{
    public abstract class SocketServer
    {
        //State variable is initiated in child class
        public NetworkStateManager ServerStateManager { get; protected set; }
        protected Action OnClientConnected, OnClientDisconnected;
        public readonly ChannelManager _channelManager;
        protected readonly ObjectMessageDispatcher _messageDispatcher = new ObjectMessageDispatcher();
        public bool Running = false;
        protected readonly SemaphoreSlim _connectionLimiter;
        protected Func<Socket> _serverSocketFactory;

        public SocketServer(int maxClients)
        {
            ServerMessageHandler.Server = this;
            _connectionLimiter= new SemaphoreSlim(maxClients,maxClients);
            _channelManager = new ChannelManager
                (
                    ()=> 
                        {
                            var Channel = CreateChannel();
                            _messageDispatcher.Bind(Channel.TCPChannel);
                            _messageDispatcher.Bind(Channel.UDPChannel);
                            return Channel;
                        }
                );
            _channelManager.ChannelClosed += (s, e) => _connectionLimiter.Release();
        }

        public void SetOnClientConnected(Action OnClientConnected) => this.OnClientConnected = OnClientConnected;
        public void SetOnClientDisconnected(Action OnClientDisconnected) 
        { 
            this.OnClientDisconnected = OnClientDisconnected;
            _channelManager.ChannelClosed += (s, e) => OnClientDisconnected();
        }

        public void Bind<TController>() => _messageDispatcher.Bind<TController>();

        public Task StartAsync(int port,CancellationToken cancellationToken)
        {
            Running = true;
            _serverSocketFactory = () =>
            {
                var EndPoint = new IPEndPoint(IPAddress.Loopback, port);

                var socket = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.Bind(EndPoint);
                socket.Listen(128);
                return socket;
            };
            return Task.Factory.StartNew(() => RunAsync(cancellationToken),cancellationToken);
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            try
            {
                Socket serverSocket = null;
                do
                {
                    if (!await _connectionLimiter.WaitAsync(1000, cancellationToken))
                    {
                        //close off Server Socket, no available connection slots.
                        try
                        {
                            serverSocket?.Close();
                            serverSocket?.Dispose();
                            serverSocket = null;
                        }
                        catch { }
                        await _connectionLimiter.WaitAsync(cancellationToken);
                    }
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        if (serverSocket == null)
                            serverSocket = _serverSocketFactory();
                        await AcceptConnection(serverSocket);
                    }
                } while (!cancellationToken.IsCancellationRequested);
            }
            catch (OperationCanceledException e)
            {
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"ServerSocket::RunAsync => {e}");
            }
        }
        public async Task BroadcastTCP<T>(Guid ExceptionId,T Message)
        {
            await _channelManager.BroadcastTCP(ExceptionId, Message).ConfigureAwait(false);
        }

        public async Task BroadcastUDP<T>(Guid ExceptionId, T Message)
        {
            await _channelManager.BroadcastUDP(ExceptionId, Message);
        }
        private async Task AcceptConnection(Socket serverSocket)
        {
            var clientsocket = await Task.Factory.FromAsync(
                new Func<AsyncCallback,object,IAsyncResult>(serverSocket.BeginAccept),
                new Func<IAsyncResult,Socket>(serverSocket.EndAccept),
                null
                ).ConfigureAwait(false);

            Console.WriteLine("SERVER :: CLIENT CONNECTION REQUEST");
            _channelManager.Accept(clientsocket);
            OnClientConnected?.Invoke();
            Console.WriteLine($"SERVER :: CLIENT CONNECTED :: Remaining Connection slots:{_connectionLimiter.CurrentCount}");
        }

        protected virtual ObjectMessageServerChannels CreateChannel() => new ObjectMessageServerChannels();
        //This is the server side loop, which has the logic about when to send message to a client.
        public abstract Task ServerLoop(Task serverTask,CancellationTokenSource cancellationTokenSource);
    }
}
