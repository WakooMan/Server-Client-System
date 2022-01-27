using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public class ManualSerializationDispatcher : MessageDispatcher<object>
    {
        public static Dictionary<int,Type> RegisteredMessageTypes = new Dictionary<int,Type>();

        protected override void AddType(Type type)
        {
            RegisteredMessageTypes.Add(((ManuallySerializedMessage)Activator.CreateInstance(type)).MessageId, type);
        }

        public override void UnBind()
        {
            base.UnBind();
            RegisteredMessageTypes.Clear();
        }

        protected override object Deserialize(Type type, object message)
            => ManualMessageSerialization.Deserialize(type, message);

        protected override TParam Deserialize<TParam>(object messageType)
            => ManualMessageSerialization.Deserialize<TParam>(messageType);

        protected override RouteAttribute GetAttribute(MethodInfo mi) => mi.GetCustomAttribute<ManualSerializationRouteAttribute>();

        protected override bool IsMatch(RouteAttribute route, object message)
            => message.ToString().Equals(route.Path);


        protected override object Serialize<TResult>(TResult result)
        => ManualMessageSerialization.Serialize(result);
    }
}
