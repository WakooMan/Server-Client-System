using System;

namespace Networking
{
    //Abstract class to Manage the network states, in implementation it is recommended to have one Client and Server Networkstate if they have different logic.
    public abstract class NetworkState<TStateEnum> where TStateEnum : Enum
    {
        //Current State of the server
        public TStateEnum CurrState { get; protected set; }
        //this will increment the CurrState variable or it changes the CurrState variable by any logic.
        public abstract void PushNextState();
        //Goes back to the previous state.
        public abstract void PopState();

        //default constructor
        public NetworkState()
        {
            CurrState = default(TStateEnum);
        }
        //constructor
        public NetworkState(TStateEnum state)
        {
            CurrState = state;
        }


    }
}