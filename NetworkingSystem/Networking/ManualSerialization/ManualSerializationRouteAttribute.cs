using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ManualSerializationRouteAttribute: RouteAttribute
    {
            public ManualSerializationRouteAttribute(string path) : base(path)
            {
            }
    }
}
