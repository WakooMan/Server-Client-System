
namespace Networking
{
    public abstract class ObjectMessageServer : Server<ObjectNetworkChannel, ObjectMessageProtocol, ObjectMessageDispatcher, object>
    {
        protected ObjectMessageServer(int maxClients) : base(maxClients)
        {
        }
    }
}
