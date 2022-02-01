using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Protobuf
{
    public class ProtobufChannel<TMessageType>: NetworkChannel<ProtobufMsgProtocol<TMessageType>,object>
    {
        public ProtobufChannel(): base() { }
    }
}
