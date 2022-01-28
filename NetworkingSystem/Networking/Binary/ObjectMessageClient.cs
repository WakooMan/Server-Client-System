
namespace Networking
{
    public abstract class ObjectMessageClient : Client<ObjectNetworkChannel, ObjectMessageProtocol, ObjectMessageDispatcher, object>
    {
        public ObjectMessageClient(string IPAddr, int _port) : base(IPAddr, _port)
        {
        }

        protected abstract override void ClientLoop();
    }
}
