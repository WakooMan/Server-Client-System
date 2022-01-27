

namespace Networking
{
    public abstract class ManualSerializationServer : Server<ManualSerializationChannel, ManualSerializationProtocol, ManualSerializationDispatcher, object>
    {
        public ManualSerializationServer(int maxClients) : base(maxClients)
        {
        }

        protected abstract override void ServerLoop();
    }
}
