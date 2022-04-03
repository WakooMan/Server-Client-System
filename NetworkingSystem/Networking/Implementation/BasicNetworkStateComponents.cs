using Networking.Implementation.Messages;
using Networking.Implementation.Messages.FromClientMessages;
using Networking.Implementation.Messages.FromServerMessages;
using System.Linq;

namespace Networking.Implementation
{
    public class BasicNetworkStateClientComponent : BaseNetworkStateComponent
    {
        public override void AddMessageHandlers(Network network)
        {
            network.Register(new Message.ServerToClientMessageHandlerDelegate<KeepAliveResponseMessage>(HandleKeepAliveResponseMessage));
            network.Register(new Message.ServerToClientMessageHandlerDelegate<NetworkDataResponseMessage>(HandleNetworkDataResponseMessage));
            network.Register(new Message.ServerToClientMessageHandlerDelegate<PlayerJoinedMessage>(HandlePlayerJoinedMessage));
            network.Register(new Message.ServerToClientMessageHandlerDelegate<PlayerDisconnectedMessage>(HandlePlayerDisconnectedMessage));
        }

        public override void RemoveMessageHandlers(Network network)
        {
            network.UnRegister(new Message.ServerToClientMessageHandlerDelegate<KeepAliveResponseMessage>(HandleKeepAliveResponseMessage));
            network.UnRegister(new Message.ServerToClientMessageHandlerDelegate<NetworkDataResponseMessage>(HandleNetworkDataResponseMessage));
            network.UnRegister(new Message.ServerToClientMessageHandlerDelegate<PlayerJoinedMessage>(HandlePlayerJoinedMessage));
            network.UnRegister(new Message.ServerToClientMessageHandlerDelegate<PlayerDisconnectedMessage>(HandlePlayerDisconnectedMessage));
        }

        private void HandleKeepAliveResponseMessage(KeepAliveResponseMessage response)
        {

        }

        private void HandleNetworkDataResponseMessage(NetworkDataResponseMessage response)
        {
            Network.Data.MainPlayer.ID = response.PlayerID;
            response.Players.ForEach(player => Network.Data.Add(player.ReturnAsPlayer()));
        }

        private void HandlePlayerJoinedMessage(PlayerJoinedMessage message)
        {
            Network.Data.Add(message.Player.ReturnAsPlayer());
        }

        private void HandlePlayerDisconnectedMessage(PlayerDisconnectedMessage message)
        {
            Network.Data.Remove(message.ID);
        }
    }

    public class BasicNetworkStateServerComponent : BaseNetworkStateComponent
    {
        public override void AddMessageHandlers(Network network)
        {
            network.Register(new Message.ClientToServerMessageHandlerDelegate<KeepAliveRequestMessage>(HandleKeepAliveRequestMessage));
            network.Register(new Message.ClientToServerMessageHandlerDelegate<NetworkDataRequestMessage>(HandleNetworkDataRequestMessage));
        }

        public override void RemoveMessageHandlers(Network network)
        {
            network.UnRegister(new Message.ClientToServerMessageHandlerDelegate<KeepAliveRequestMessage>(HandleKeepAliveRequestMessage));
            network.UnRegister(new Message.ClientToServerMessageHandlerDelegate<NetworkDataRequestMessage>(HandleNetworkDataRequestMessage));
        }

        private void HandleKeepAliveRequestMessage(INetworkChannel Channel, KeepAliveRequestMessage request)
        {
            Network.Server.SendTo(Channel.Id, new KeepAliveResponseMessage(new Result(Status.Success)));
        }

        private void HandleNetworkDataRequestMessage(INetworkChannel Channel, NetworkDataRequestMessage request)
        {
            Player player = request.Player.ReturnAsPlayer();
            player.ID = Channel.Id;
            Network.Data.Add(player);
            Network.Server.SendTo(Channel.Id, new NetworkDataResponseMessage(Channel.Id, Network.Data.Players.ToList()));
            Network.Server.Broadcast(Channel.Id,new PlayerJoinedMessage(player));
        }
    }
}
