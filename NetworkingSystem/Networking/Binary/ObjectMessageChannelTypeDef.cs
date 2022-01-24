using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{

    public class ObjectNetworkChannel : NetworkChannel<ObjectMessageProtocol, object>
    {
        public ObjectNetworkChannel(NetPeer peer) : base(peer)
        {
        }
    }
}
