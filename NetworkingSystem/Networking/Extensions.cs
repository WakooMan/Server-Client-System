using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public static class Extensions
    {
        public static INetworkChannel GetConnection(this NetPeer peer)
        {
            return (INetworkChannel)peer.Tag;
        }
    }
}
