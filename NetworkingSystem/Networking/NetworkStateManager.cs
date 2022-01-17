using System;

namespace Networking
{
    //Abstract class to Manage the network states, in implementation it is recommended to have one Client and Server Networkstate if they have different logic.
    public abstract class NetworkStateManager
    {
        protected SocketClient Client { get; private set; }
        protected SocketServer Server { get; private set; }


        //Current State of the server
        public NetworkState CurrState { get; protected set; }
        public int CurrStateNum { get; protected set; }
        public string[] StateNames { get; protected set; }
        //this will increment the CurrState variable or it changes the CurrState variable by any logic.
        public abstract void PushNextState();
        //Goes back to the previous state.
        public abstract void PopState();

        //default constructor
        public NetworkStateManager(SocketServer Server)
        {
            this.Server = Server;
            Client = null;
            CurrState = null;
        }

        public NetworkStateManager(SocketClient Client)
        {
            this.Client = Client;
            Server = null;
            CurrState = null;
        }
        //constructor
        public NetworkStateManager(NetworkState state,SocketServer Server)
        {
            this.Server = Server;
            Client = null;
            CurrState = state;
        }

        public NetworkStateManager(NetworkState state, SocketClient Client)
        {
            this.Client = Client;
            Server = null;
            CurrState = state;
        }
    }
}