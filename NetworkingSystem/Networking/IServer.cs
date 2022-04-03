using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public interface IServer: IUpdatable
    {
        bool CanPlayerJoin();
        void Connected(INetworkChannel Channel);
        void AddOnClientConnectedEvent(Action OnClientConnected);
        void AddOnClientDisconnectedEvent(Action OnClientDisconnected);
        void SendTo<T>(Guid Id, T Message) where T: class;
        void Broadcast<T>(Guid ExceptionId, T Message) where T : class;  

    }
}
