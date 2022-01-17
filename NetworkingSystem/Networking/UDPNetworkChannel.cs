using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Networking
{
    public class UDPNetworkChannel<TProtocol, TMessageType> : IDisposable, INetworkChannel where TProtocol : Protocol<TMessageType>, new ()
    {
        private UdpClient Sender { get; set; }
        private UdpClient Receiver { get; set; }

        protected bool isDisposed = false;
        protected bool isClosed = false;
        public bool IsClosed { get { return isClosed; } }
        protected readonly TProtocol protocol = new TProtocol();
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private Task _receiveLoopTask;
        private Func<TMessageType, Task> messageCallback;
        public Guid Id { get; private set; }

        public UDPNetworkChannel(Guid id,Action receivedMessage) { Id = id; ReceivedMessage = receivedMessage; }

        private Action ReceivedMessage { get; set; }

        public void Attach(string ipAddress, int sendPort, int receivePort)
        {
            Sender = new UdpClient();
            Sender.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), sendPort));
            Receiver = new UdpClient(receivePort);
            _receiveLoopTask = Task.Run(ReceiveLoop, cancellationTokenSource.Token);
        }
        public void Close()
        {
            if (!isClosed)
            {
                isClosed = true;
                cancellationTokenSource.Cancel();
                Sender.Close();
                Receiver.Close();
            }
        }

        public void OnMessage(Func<TMessageType, Task> callbackHandler) => messageCallback = callbackHandler;

        protected virtual async Task ReceiveLoop()
        {
            try
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    var msg = await protocol.ReceiveAsync(Receiver).ConfigureAwait(false);
                    ReceivedMessage?.Invoke();
                    await messageCallback(msg).ConfigureAwait(false);
                }
            }
            catch (System.ObjectDisposedException e)
            {
                Console.WriteLine($"UDP Channel {Id} disconnected.");
                Close();
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine($"UDP Channel {Id} disconnected.");
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
            await protocol.SendAsync(Sender, Message);
        }

        ~UDPNetworkChannel() => Dispose(false);
        public void Dispose() => Dispose(true);

        public void Dispose(bool isDisposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                Close();

                //TODO: Clean up stuff

                if (isDisposing)
                    GC.SuppressFinalize(this);
            }
        }
    }
}