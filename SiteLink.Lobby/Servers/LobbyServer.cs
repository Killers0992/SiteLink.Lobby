using Lobby.Worlds;
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
    }, true) { }

    public override bool OnClientConnecting(Client client) => true;
    public override void OnClientReady(Client client) => client.SpawnObjects();
    public override void OnClientSpawnPlayer(Client client) => client.World = new LobbyWorld();
}
