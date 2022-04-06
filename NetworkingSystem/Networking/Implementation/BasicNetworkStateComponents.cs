using Networking.Implementation.Messages;
using Networking.Implementation.Messages.FromClientMessages;
using Networking.Implementation.Messages.FromServerMessages;
using System.Linq;

namespace Networking.Implementation
{
    public class BasicNetworkStateClientComponent : BaseClientNetworkStateComponent
    {

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

    public class BasicNetworkStateServerComponent : BaseServerNetworkStateComponent
    {
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
