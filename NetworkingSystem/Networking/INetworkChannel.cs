using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Networking
{
    public interface INetworkChannel
    {
        Guid Id { get; }
        void Close();
        void Dispose();
        Task SendAsync<T>(T Message);
        
    }
}