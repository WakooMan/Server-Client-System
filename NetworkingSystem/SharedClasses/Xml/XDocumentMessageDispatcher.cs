using System;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SharedClasses{
    public class XDocumentMessageDispatcher : MessageDispatcher<XDocument>
    {
        protected override TParam Deserialize<TParam>(XDocument messageType)
            => XmlSerialization.Deserialize<TParam>(messageType);

        protected override XDocument Serialize<TResult>(TResult result) => XmlSerialization.Serialize(result);

        protected override RouteAttribute GetAttribute(MethodInfo mi) => mi.GetCustomAttribute<XPathRouteAttribute>();

        protected override bool IsMatch(RouteAttribute route, XDocument message)
            => (message.XPathEvaluate($"boolean({route.Path})") as bool?) ?? false;

        protected override object Deserialize(Type type, XDocument message)
            => XmlSerialization.Deserialize(type,message);
    }
}
