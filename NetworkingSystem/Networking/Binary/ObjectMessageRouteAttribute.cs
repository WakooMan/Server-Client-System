using System;

namespace Networking
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ObjectMessageRouteAttribute : RouteAttribute
    {
        public ObjectMessageRouteAttribute(string path) : base(path)
        {
        }
    }
}
