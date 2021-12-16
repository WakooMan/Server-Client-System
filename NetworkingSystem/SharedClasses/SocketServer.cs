
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SharedClasses
{
    public abstract class SocketServer<TChannelType,TProtocol,TMessageType,TMessageDispatcher>
        where TChannelType : NetworkChannel<TProtocol,TMessageType>,new()
        where TProtocol: Protocol<TMessageType>,new()
        where TMessageDispatcher: MessageDispatcher<TMessageType>,new()
        where TMessageType: class,new()

    {
        readonly ChannelManager _channelManager;
        readonly TMessageDispatcher _messageDispatcher = new TMessageDispatcher();

        readonly SemaphoreSlim _connectionLimiter;
        Func<Socket> _serverSocketFactory;

        public SocketServer(int maxClients)
        {
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

        private async Task AcceptConnection(Socket serverSocket)
        {
            var clientsocket = await Task.Factory.FromAsync(
                new Func<AsyncCallback,object,IAsyncResult>(serverSocket.BeginAccept),
                new Func<IAsyncResult,Socket>(serverSocket.EndAccept),
                null
                ).ConfigureAwait(false);

            Console.WriteLine("SERVER :: CLIENT CONNECTION REQUEST");
            _channelManager.Accept(clientsocket);
            Console.WriteLine($"SERVER :: CLIENT CONNECTED :: Remaining Connection slots:{_connectionLimiter.CurrentCount}");
        }

        protected virtual TChannelType CreateChannel() => new TChannelType();
    }
}
