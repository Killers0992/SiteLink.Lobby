using Lobby.Models;
using Microsoft.Extensions.DependencyInjection;
using SiteLink.API;
using SiteLink.API.Core;
using SiteLink.API.Plugins;

namespace Lobby;

public class MainClass : Plugin<Config>
{
    public static MainClass Singleton { get; private set; }
    public static LobbyServer ServerInstance { get; private set; }

    public static Dictionary<string, PortalInfo> ServerByNames = new Dictionary<string, PortalInfo>();

    public static string GetPortalTextByServer(string serverName)
    {
        if (ServerByNames.TryGetValue(serverName, out var portal))
        {
            return portal.Text;
        }

        return "Unknown";
    }

    public override string Name { get; } = "Lobby";

    public override string Description { get; } = "Adds lobby to SiteLink";

    public override string Author { get; } = "Killers0992";

    public override Version Version { get; } = new Version(1, 0, 2);

    public override Version ApiVersion { get; } = new Version(SiteLinkAPI.ApiVersionText);

    public override void OnLoad(IServiceCollection collection)
    {
        Singleton = this;
        ServerInstance = new LobbyServer();
        Server.Register(ServerInstance);
    }

    public override void OnUnload()
    {
        ServerInstance?.SharedWorld.Dispose();
        ServerInstance = null;
        Singleton = null;
    }

    public override void LoadConfig()
    {
        base.LoadConfig();

        ServerByNames.Clear();

        foreach (var portal in Config.Portals)
        {
            ServerByNames[portal.TargetServer] = portal;
        }
    }
}
