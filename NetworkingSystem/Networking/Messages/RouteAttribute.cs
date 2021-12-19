using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class RouteAttribute: Attribute
    {
        public string Path { get; }

        public RouteAttribute(string path) => Path = path;
    }
}
