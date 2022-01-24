using System;

namespace Networking
{
    //Abstract class to Manage the network states, in implementation it is recommended to have one Client and Server Networkstate if they have different logic.
    public abstract class NetworkStateManager
    {

        //Current State of the server
        public NetworkState CurrState { get; protected set; }
        public int CurrStateNum { get; protected set; }
        public string[] StateNames { get; protected set; }
        //this will increment the CurrState variable or it changes the CurrState variable by any logic.
        public abstract void PushNextState();
        //Goes back to the previous state.
        public abstract void PopState();
        
        //constructor
        public NetworkStateManager(NetworkState state)
        {
            CurrState = state;
        }
    }
}