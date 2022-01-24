using System;
using System.Collections.Concurrent;

namespace Networking
{
    public class ChannelManager
    {
        public readonly ConcurrentDictionary<Guid, INetworkChannel> Channels = new ConcurrentDictionary<Guid, INetworkChannel>();
        

        public event EventHandler ChannelClosed;

        public ChannelManager()
        {
        }

        public void Accept(INetworkChannel Channel)
        {
            Channels.TryAdd(Channel.Id,Channel);
            Channel.Closed += (s, e) =>
            {
                Channels.TryRemove(Channel.Id, out var _);
                ChannelClosed?.Invoke(this, EventArgs.Empty);
            };
        }

        public void Broadcast<T>(Guid ExceptionId, T Message,EDeliveryMethod eMethod)
        {
            foreach (Guid Id in Channels.Keys)
            {
                if (!Id.Equals(ExceptionId))
                    Channels[Id].Send(Message,eMethod);
            }
        }

        public void SendTo<T>(Guid id, T message, EDeliveryMethod eMethod)
        {
            foreach (Guid Id in Channels.Keys)
            {
                if (Id.Equals(id))
                    Channels[Id].Send(message, eMethod);
            }
        }
    }
}
