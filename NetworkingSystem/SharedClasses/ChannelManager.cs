﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SharedClasses
{
    public class ChannelManager
    {
        public readonly ConcurrentDictionary<Guid, INetworkChannel> Channels = new ConcurrentDictionary<Guid, INetworkChannel>();
        private readonly Func<INetworkChannel> _channelFactory;

        const int GROOMING_INTERVAL_MINUTES = 1;
        private readonly  System.Timers.Timer _groomer = new System.Timers.Timer(GROOMING_INTERVAL_MINUTES*60*1000);

        public event EventHandler ChannelClosed;

        public ChannelManager(Func<INetworkChannel> ChannelFactory)
        {
            _channelFactory = ChannelFactory;
            _groomer.Elapsed += OnGroomerElapsedEventHandler;
            _groomer.Start();
        }

        private void OnGroomerElapsedEventHandler(object sender, ElapsedEventArgs e)
        {
            _groomer.Stop();
            Console.WriteLine("BEGIN SOCKET GROOMING");
            int SocketsGroomed = 0;
            try
            {
                var DeadChannels = new List<Guid>();

                var delta = DateTime.UtcNow.Subtract(new TimeSpan(0,GROOMING_INTERVAL_MINUTES,0));
                foreach (var key in Channels.Keys)
                {
                    var channel = Channels[key];
                    var mostrecent = DateTime.Compare(channel.LastReceived,channel.LastSent)>0? channel.LastReceived :channel.LastSent;
                    if (DateTime.Compare(delta, mostrecent) < 0)
                        DeadChannels.Add(key);
                }

                foreach (var key in DeadChannels)
                {
                    Console.WriteLine($"Closing/Removing Dead Channel:{key}");
                    var channel = Channels[key];
                    channel.Dispose();
                    SocketsGroomed++;
                }
            }
            finally
            {
                _groomer.Start();
            }
            Console.WriteLine($"END SOCKET GROOMING: {SocketsGroomed} Sockets Groomed");
        }

        public void Accept(Socket socket)
        {
            var channel = _channelFactory();
            Channels.TryAdd(channel.Id,channel);
            channel.Closed += (s, e) =>
            {
                Channels.TryRemove(channel.Id, out var _);
                ChannelClosed?.Invoke(this, EventArgs.Empty);
            };
            channel.Attach(socket);
        }

        public async Task Broadcast<T>(Guid ExceptionId, T Message)
        {
            foreach (Guid Id in Channels.Keys)
            {
                if (!Id.Equals(ExceptionId))
                    await Channels[Id].SendAsync(Message).ConfigureAwait(false);
            }
        }
    }
}
