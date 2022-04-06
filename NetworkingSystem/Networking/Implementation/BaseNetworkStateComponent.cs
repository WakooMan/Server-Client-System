namespace Networking.Implementation
{
    public abstract class BaseNetworkStateComponent
    {
        public abstract void AddMessageHandlers(Network network);
        public abstract void RemoveMessageHandlers(Network network);
    }
}
