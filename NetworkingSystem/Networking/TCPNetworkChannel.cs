using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Networking{
    public abstract class TCPNetworkChannel<TProtocol, TMessageType> : IDisposable, INetworkChannel where TProtocol : Protocol<TMessageType>, new()
    {
        protected bool isDisposed = false;
        protected bool isClosed = false;
        protected readonly TProtocol protocol = new TProtocol();
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private NetworkStream _networkStream;
        private Task _receiveLoopTask;
        private Func<TMessageType, Task> messageCallback;
        private Action ReceivedMessage { get; set; }

        public TCPNetworkChannel(Guid id,Action receivedMessage) { Id = id; ReceivedMessage = receivedMessage; }
        public Guid Id { get; private set; }

        public void Attach(Socket socket)
        {
            _networkStream = new NetworkStream(socket, true);
            _receiveLoopTask = Task.Run(ReceiveLoop, cancellationTokenSource.Token);
        }
        public void Close()
        {
            if (!isClosed)
            {
                isClosed = true;
                cancellationTokenSource.Cancel();
                _networkStream?.Close();
            }
        }

        public void OnMessage(Func<TMessageType, Task> callbackHandler) => messageCallback = callbackHandler;

        protected virtual async Task ReceiveLoop()
        {
            try
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    var msg = await protocol.ReceiveAsync(_networkStream).ConfigureAwait(false);
                    ReceivedMessage?.Invoke();
                    await messageCallback(msg).ConfigureAwait(false);
                }
            }
            catch (System.ObjectDisposedException e)
            {
                Console.WriteLine($"TCP Channel {Id} disconnected.");
                Close();
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine($"TCP Channel {Id} disconnected.");
                Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"NetworkChannel::ReceiveLoop => {e}");
                Close();
            }
        }

        public async Task SendAsync<T>(T Message)
        { 
            await protocol.SendAsync(_networkStream, Message).ConfigureAwait(false);
        }

        ~TCPNetworkChannel() => Dispose(false);
        public void Dispose() => Dispose(true);

        public void Dispose(bool isDisposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;


                //TODO: Clean up stuff

                if (isDisposing)
                    GC.SuppressFinalize(this);
            }
        }
    }
}
