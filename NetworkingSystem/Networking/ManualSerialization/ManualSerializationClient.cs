using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public abstract class ManualSerializationClient : Client<ManualSerializationChannel, ManualSerializationProtocol, ManualSerializationDispatcher, object>
    {
        public ManualSerializationClient(string IPAddr, int _port) : base(IPAddr, _port)
        {
        }

        protected abstract override void ClientLoop();
    }
}
