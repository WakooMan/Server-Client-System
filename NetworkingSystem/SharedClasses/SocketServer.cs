
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Networking
{
    public abstract class SocketServer<TStateEnum>
        where TStateEnum : Enum
    {
        //State variable is initiated in child class
        public NetworkState<TStateEnum> ServerState { get; }
        private Action OnClientConnected,OnClientDisconnected;
        protected readonly ChannelManager _channelManager;
        protected readonly XDocumentMessageDispatcher _messageDispatcher = new XDocumentMessageDispatcher();

        protected readonly SemaphoreSlim _connectionLimiter;
        protected Func<Socket> _serverSocketFactory;

        public SocketServer(int maxClients,Action _onClientConnected,Action _onClientDisconnected)
        {
            ServerMessageHandler<TStateEnum>.Server = this;
            OnClientConnected = _onClientConnected;
            OnClientDisconnected = _onClientDisconnected;
            _connectionLimiter= new SemaphoreSlim(maxClients,maxClients);
            _channelManager = new ChannelManager
                (
                    ()=> 
                        {
                            var Channel = CreateChannel();
                            _messageDispatcher.Bind(Channel);
                            return Channel;
                        }
                );
            _channelManager.ChannelClosed += (s, e) => _connectionLimiter.Release();
            _channelManager.ChannelClosed+=(s,e) => OnClientDisconnected();
        }

        public void Bind<TController>() => _messageDispatcher.Bind<TController>();

        public Task StartAsync(int port,CancellationToken cancellationToken)
        {
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
        public async Task Broadcast<T>(Guid ExceptionId,T Message)
        {
            await _channelManager.Broadcast(ExceptionId, Message).ConfigureAwait(false);
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

        protected virtual XmlChannel CreateChannel() => new XmlChannel();
        //This is the server side loop, which has the logic about when to send message to a client.
        public abstract void ServerLoop(Task serverTask,CancellationTokenSource cancellationTokenSource);
    }
}
