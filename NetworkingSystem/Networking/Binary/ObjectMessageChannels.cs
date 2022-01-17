using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public class ObjectMessageServerChannels : IServerNetworkChannels<ObjectMessageUDPChannel,ObjectMessageTCPChannel, ObjectMessageProtocol,object>
    {
        public Guid Id { get; private set; }
        public ObjectMessageTCPChannel TCPChannel { get; private set; }
        public ObjectMessageUDPChannel UDPChannel { get; private set; }

        public DateTime LastSent { get; private set; }

        public DateTime LastReceived { get; private set; }

        public bool IsClosed { get; private set; } = false;

        public bool IsDisposed { get; private set; } = false;

        public event EventHandler Closed;
        public ObjectMessageServerChannels()
        {
            Id = Guid.NewGuid();
            TCPChannel = new ObjectMessageTCPChannel(Id, () => LastReceived = DateTime.UtcNow);
            UDPChannel = new ObjectMessageUDPChannel(Id, () => LastReceived = DateTime.UtcNow);
        }

        public void Attach(Socket socket)
        {
            TCPChannel.Attach(socket);
            UDPChannel.Attach("127.0.0.1", 9002, 9001);
        }

        public void Close()
        {
            if (!IsClosed)
            {
                IsClosed = true;
                TCPChannel.Close();
                UDPChannel.Close();
                Closed?.Invoke(this, EventArgs.Empty);
            }
        }

        public async Task SendUDP<T>(T message)
        {
            await UDPChannel.SendAsync(message).ConfigureAwait(false);
            LastSent = DateTime.UtcNow;
        }

        public async Task SendTCP<T>(T message)
        {
            await TCPChannel.SendAsync(message).ConfigureAwait(false);
            LastSent = DateTime.UtcNow;
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                UDPChannel.Dispose();
                TCPChannel.Dispose();
            }
        }
    }

    public class ObjectMessageClientChannels: IClientNetworkChannels<ObjectMessageUDPChannel,ObjectMessageClientTCPChannel,ObjectMessageProtocol,object>
    {
        public Guid Id { get; private set; }
        public ObjectMessageClientTCPChannel TCPChannel { get; private set; }
        public ObjectMessageUDPChannel UDPChannel { get; private set; }

        public bool IsClosed { get; private set; } = false;

        public bool IsDisposed { get; private set; } = false;

        public DateTime LastSent { get; private set; }

        public DateTime LastReceived { get; private set; }

        public ObjectMessageClientChannels()
        {
            Id = Guid.NewGuid();
            TCPChannel = new ObjectMessageClientTCPChannel(Id, () => LastReceived=DateTime.UtcNow);
            UDPChannel = new ObjectMessageUDPChannel(Id, () => LastReceived = DateTime.UtcNow);
        }

        public void Attach(Socket socket)
        {
            TCPChannel.Attach(socket);
            UDPChannel.Attach("127.0.0.1",9001,9002);
        }

        public void Close()
        {
            if (!IsClosed)
            {
                IsClosed = true;
                TCPChannel.Close();
                UDPChannel.Close();
            }
        }

        public async Task SendUDP<T>(T message)
        {
            await UDPChannel.SendAsync(message).ConfigureAwait(false);
            LastSent = DateTime.UtcNow;
        }

        public async Task SendTCP<T>(T message)
        {
            await TCPChannel.SendAsync(message).ConfigureAwait(false);
            LastSent = DateTime.UtcNow;
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                UDPChannel.Dispose();
                TCPChannel.Dispose();
            }
        }

        public async Task ConnectAsync(IPEndPoint EndPoint)
        {
            await TCPChannel.ConnectAsync(EndPoint);
            UDPChannel.Attach("127.0.0.1", 9001, 9002);
        }
    }
}
