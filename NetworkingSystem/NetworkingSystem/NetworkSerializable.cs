using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingSystem
{
    public interface NetworkSerializable
    {
        byte[] Serialize();
        void Deserialize(byte[] bytes);
    }
}
