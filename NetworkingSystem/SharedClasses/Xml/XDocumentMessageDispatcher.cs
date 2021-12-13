using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SharedClasses{
    public class XDocumentMessageDispatcher : MessageDispatcher<XDocument>
    {
        

        public override void Register<TParam, TResult>(Func<TParam, Task<TResult>> target)
        {
            if (!HasAttribute(target.Method))
                throw new Exception("Missing Required Route Attribute");
            var XPathRouteExpression = GetXPathRoute(target.Method);

            var wrapper = new Func<XDocument, Task<XDocument>>(async xml =>
            {
                var Param = XmlSerialization.Deserialize<TParam>(xml);
                var result = await target(Param);

                if (result != null)
                    return XmlSerialization.Serialize(result);
                else
                    return null;
            });
            AddHandler(GetAttribute(target.Method),wrapper);
        }

        public override void Register<TParam>(Func<TParam, Task> target)
        {
            if (!HasAttribute(target.Method))
                throw new Exception("Missing Required Route Attribute");
            var XPathRouteExpression = GetXPathRoute(target.Method);

            var wrapper = new Func<XDocument, Task<XDocument>>(async xml =>
            {
                var Param = XmlSerialization.Deserialize<TParam>(xml);
                await target(Param);
                return null;
            });
            AddHandler(GetAttribute(target.Method), wrapper);
        }

        protected RouteAttribute GetAttribute(MethodInfo mi) => mi.GetCustomAttribute<RouteAttribute>();

        protected bool HasAttribute(MethodInfo mi)
            => GetAttribute(mi) != null;

        protected override bool IsMatch(RouteAttribute route, XDocument message)
            => (message.XPathEvaluate($"boolean({route.Path})") as bool?) ?? false;

        private string GetXPathRoute(MethodInfo methodInfo)
        {
            var routeAttribute = methodInfo.GetCustomAttribute<RouteAttribute>();
            if (routeAttribute == null)
                throw new ArgumentException($"Method {methodInfo.Name} missing required RouteAttribute.");
            return $"boolean({routeAttribute.Path})";
        }
    }
}
