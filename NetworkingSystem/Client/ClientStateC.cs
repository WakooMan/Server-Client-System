using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class ClientStateC : NetworkState<ClientStates>
    {
        public ClientStateC() :base(){ }
        public ClientStateC(ClientStates initState) : base(initState) { }

        public override void PopState()
            //If the Current State is the first state we can't go back to the previous State.
            => CurrState = CurrState>0? CurrState - 1:0;

        public override void PushNextState()
            => CurrState++;
    }
}
