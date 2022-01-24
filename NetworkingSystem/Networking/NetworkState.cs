using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public abstract class NetworkState
    {
        public bool IsThisState(string StateName)
        {
            return StateName == ToString();
        }
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public abstract string ToString();
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        public abstract void ExecuteStuff();
    }
}
