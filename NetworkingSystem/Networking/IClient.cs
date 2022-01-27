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
        void Bind<TController>();
        void Disconnect();
        void StartClientLoopOnNewThread();
        void Send<T>(T msg) where T : Message;
    }
}
