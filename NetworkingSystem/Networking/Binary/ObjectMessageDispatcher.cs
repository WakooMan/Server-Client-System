using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public class ObjectMessageDispatcher: MessageDispatcher<object>
    {
        protected override object Deserialize(Type type, object message)
            => ObjectMessageSerialization.Deserialize(type, message);

        protected override TParam Deserialize<TParam>(object messageType)
            => ObjectMessageSerialization.Deserialize<TParam>(messageType);

        protected override RouteAttribute GetAttribute(MethodInfo mi)=> mi.GetCustomAttribute<ObjectMessageRouteAttribute>();

        protected override bool IsMatch(RouteAttribute route, object message)
            => message.ToString().Equals(route.Path);

        protected override object Serialize<TResult>(TResult result)
        => ObjectMessageSerialization.Serialize(result);
    }
}
