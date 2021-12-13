using SharedClasses.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SharedClasses.Xml
{
    public class XDocumentMessageDispatcher : MessageDispatcher<XDocument>
    {
        readonly List<(string xPathExpression, Func<XDocument, Task<XDocument>> targetMethod)> _handlers = new List<(string xPathExpression, Func<XDocument, Task<XDocument>> targetMethod)>(); 
        public override async Task<XDocument> DispatchAsync(XDocument message)
        {
            foreach ( var (xpath,target) in _handlers)
            {
                if ((message.XPathEvaluate(xpath) as bool?) == true)
                {
                    return await target(message);
                }
            }
            //No Handler what to do?
            return null;
        }

        public override void Register<TParam, TResult>(Func<TParam, Task<TResult>> target)
        {
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
            _handlers.Add((XPathRouteExpression,wrapper));
        }

        public override void Register<TParam>(Func<TParam, Task> target)
        {
            var XPathRouteExpression = GetXPathRoute(target.Method);

            var wrapper = new Func<XDocument, Task<XDocument>>(async xml =>
            {
                var Param = XmlSerialization.Deserialize<TParam>(xml);
                await target(Param);
                return null;
            });
            _handlers.Add((XPathRouteExpression, wrapper));
        }

        private string GetXPathRoute(MethodInfo methodInfo)
        {
            var routeAttribute = methodInfo.GetCustomAttribute<RouteAttribute>();
            if (routeAttribute == null)
                throw new ArgumentException($"Method {methodInfo.Name} missing required RouteAttribute.");
            return $"boolean({routeAttribute.Path})";
        }
    }
}
