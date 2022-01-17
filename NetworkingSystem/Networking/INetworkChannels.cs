using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public interface INetworkChannels
    {
        void Attach(Socket socket);
        void Close();

        DateTime LastSent { get; }
        DateTime LastReceived { get; }

        bool IsClosed { get; }
        bool IsDisposed { get; }

        void Dispose();
        Task SendUDP<T>(T message);
        Task SendTCP<T>(T message);
    }
}
