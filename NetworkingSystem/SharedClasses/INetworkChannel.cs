using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SharedClasses
{
    public interface INetworkChannel
    {
        Guid Id { get; }

        DateTime LastSent { get; }
        DateTime LastReceived { get; }

        event EventHandler Closed;
        void Attach(Socket socket);
        void Close();
        void Dispose();
        Task SendAsync<T>(T Message);
    }
}