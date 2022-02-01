using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Protobuf
{
    public class ProtobufMsgDispatcher : MessageDispatcher<object>
    {
        protected override object Deserialize(Type type, object message) => message;

        protected override TParam Deserialize<TParam>(object messageType) => (TParam)messageType;

        protected override RouteAttribute GetAttribute(MethodInfo mi) => mi.GetCustomAttribute<ProtobufMsgRouteAttribute>();

        protected override bool IsMatch(RouteAttribute route, object message) => message.ToString().Equals(route.Path);

        protected override object Serialize<TResult>(TResult result) => result;
    }
}
