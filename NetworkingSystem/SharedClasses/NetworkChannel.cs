using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedClasses
{
    public abstract class NetworkChannel<TProtocol, TMessageType> : IDisposable
        where TProtocol : Protocol<TMessageType>,new()
    {
        protected bool isDisposed = false;
        private readonly TProtocol protocol = new TProtocol();
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private NetworkStream _networkStream;
        private Task _receiveLoopTask;
        private Func<TMessageType,Task> messageCallback;


        public void Attach(Socket socket)
        {
            _networkStream = new NetworkStream(socket,true);

            _receiveLoopTask = Task.Run(ReceiveLoop,cancellationTokenSource.Token);
        }
        public void Close()
        {
            cancellationTokenSource.Cancel();
            _networkStream.Close();
        }

        public void OnMessage(Func<TMessageType, Task> callbackHandler) => messageCallback = callbackHandler;
  
        protected virtual async Task ReceiveLoop()
        {
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                //TODO: Pass Cancellation Token to Protocol Methods
                var msg = await protocol.ReceiveAsync(_networkStream).ConfigureAwait(false);
                await messageCallback(msg).ConfigureAwait(false);
            }
        }

        public async Task SendAsync<T>(T Message)
            => await protocol.SendAsync(_networkStream,Message).ConfigureAwait(false);

        ~NetworkChannel() => Dispose(false);
        public void Dispose() => Dispose(true);

        public void Dispose(bool isDisposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;


                //TODO: Clean up stuff

                if(isDisposing)
                    GC.SuppressFinalize(this);
            }
        }
    }
}
