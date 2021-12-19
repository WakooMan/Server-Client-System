using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class JsonSocketServer: SocketServer<JsonChannel,JsonMessageProtocol,JObject,JsonMessageDispatcher>
    {
        public JsonSocketServer(int maxClients,Action ClientConnected,Action ClientDisconnected):base(maxClients,ClientConnected,ClientDisconnected) { }
    }
}
