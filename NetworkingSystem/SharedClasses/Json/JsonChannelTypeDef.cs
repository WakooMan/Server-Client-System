using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class JsonChannel : NetworkChannel<JsonMessageProtocol, JObject> { }
    public class JsonClientChannel : ClientChannel<JsonMessageProtocol, JObject> { }
}
