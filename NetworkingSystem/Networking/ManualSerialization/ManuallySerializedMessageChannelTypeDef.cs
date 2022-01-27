using LiteNetLib;

namespace Networking
{
    public class ManualSerializationChannel : NetworkChannel<ManualSerializationProtocol, object>
    {
        public ManualSerializationChannel() : base()
        {
        }
    }
}
