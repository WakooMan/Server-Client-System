using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public interface IClient: IUpdatable
    {
        bool IsConnected { get; }
        void Disconnect();
        void Send<T>(T msg) where T : class;
    }
}
