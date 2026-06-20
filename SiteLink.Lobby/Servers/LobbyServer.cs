using Lobby.Worlds;
using Mirror;
using SiteLink.API.Core;
using SiteLink.API.Models;
using SiteLink.API.Networking;

namespace Lobby;

public class LobbyServer : Server
{
    public LobbyServer() : base("Lobby", new ServerSettings()
    {
        DisplayName = "Lobby",
        Address = "-local-",
        Port = 7777,
    }, true)
    { }

    public override bool OnSessionConnecting(Session session) => true;

    public override void OnSessionReady(Session session)
    {
        session.Connection.AsServer.Send(w =>
        {
            w.WriteUShort(NetworkMessages.ObjectSpawnStartedMessage);
        });

        session.World = new LobbyWorld();

        session.Connection.AsServer.Send(w =>
        {
            w.WriteUShort(NetworkMessages.ObjectSpawnFinishedMessage);
        });
    }
}
