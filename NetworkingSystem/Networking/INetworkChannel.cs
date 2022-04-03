using System;

namespace Networking
{
    public interface INetworkChannel
    {
        Guid Id { get; }
        event EventHandler Closed;
        int FragmentLength { get; }
        int MaxPackageLength { get; }
        int Latency { get; set; }
        bool IsClosed { get; }

        void OnMessage<TMessageType>(Action<INetworkChannel,TMessageType> p);
        void Close();
        void Dispose();

        void Send<T>(T Message,EDeliveryMethod eMethod) where T: class;

        void Receive(byte [] bytes);
        
    }
}