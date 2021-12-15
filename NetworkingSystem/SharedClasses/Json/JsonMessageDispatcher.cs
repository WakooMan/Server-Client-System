using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class JsonMessageDispatcher : MessageDispatcher<JObject>
    {
        protected override TParam Deserialize<TParam>(JObject messageType)
            => JsonSerialization.Deserialize<TParam>(messageType);

        protected override object Deserialize(Type type, JObject message)
            => JsonSerialization.ToObject(type, message);

        protected override RouteAttribute GetAttribute(MethodInfo mi)
            => mi.GetCustomAttribute<JsonRouteAttribute>();

        protected override bool IsMatch(RouteAttribute route, JObject message)
            => message.SelectToken(route.Path)?.ToString() == (route as JsonRouteAttribute)?.Value;

        protected override JObject Serialize<TResult>(TResult result)
            => JsonSerialization.Serialize(result);
    }
}
