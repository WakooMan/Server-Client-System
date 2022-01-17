using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public abstract class ServerMessageHandler
    {
        public static SocketServer Server { get; set; }
    }
}
