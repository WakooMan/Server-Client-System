using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedClasses{
    public abstract class NetworkChannel<TProtocol, TMessageType> : IDisposable, INetworkChannel where TProtocol : Protocol<TMessageType>, new()
    {
        protected bool isDisposed = false;
        protected bool isClosed = false;
        private readonly TProtocol protocol = new TProtocol();
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private NetworkStream _networkStream;
        private Task _receiveLoopTask;
        private Func<TMessageType, Task> messageCallback;

        public event EventHandler Closed;

        public Guid Id { get; } = Guid.NewGuid();

        public DateTime LastSent { get; protected set; }

        public DateTime LastReceived { get; protected set; }

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
                Closed?.Invoke(this, EventArgs.Empty);
            }
        }

        public void OnMessage(Func<TMessageType, Task> callbackHandler) => messageCallback = callbackHandler;

        protected virtual async Task ReceiveLoop()
        {
            try
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    //TODO: Pass Cancellation Token to Protocol Methods
                    var msg = await protocol.ReceiveAsync(_networkStream).ConfigureAwait(false);
                    LastReceived = DateTime.UtcNow;
                    await messageCallback(msg).ConfigureAwait(false);
                }
            }
            catch (System.ObjectDisposedException e)
            {
                Console.WriteLine($"Channel {Id} disconnected.");
                Close();
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine($"Channel {Id} disconnected.");
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
            LastSent = DateTime.UtcNow;
        }

        ~NetworkChannel() => Dispose(false);
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
