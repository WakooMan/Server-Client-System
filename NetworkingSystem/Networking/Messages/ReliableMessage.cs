using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    [Serializable]
    public abstract class ReliableMessage : Message
    {
        protected ReliableMessage(int id, MessageType type) : base(id, type, EDeliveryMethod.Reliable)
        {
        }
    }
}
