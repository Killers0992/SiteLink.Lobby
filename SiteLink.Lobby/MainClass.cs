using Microsoft.Extensions.DependencyInjection;
using SiteLink.API;
using SiteLink.API.Core;
using SiteLink.API.Plugins;

namespace Lobby;

public class MainClass : Plugin<Config>
{
    public static MainClass Singleton { get; private set; }

    public override string Name { get; } = "Lobby";

    public override string Description { get; } = "Adds lobby to SiteLink";

    public override string Author { get; } = "Killers0992";

    public override Version Version { get; } = new Version(1, 0, 1);

    public override Version ApiVersion { get; } = new Version(SiteLinkAPI.ApiVersionText);

    public override void OnLoad(IServiceCollection collection)
    {
        Singleton = this;
        Server.Register(new LobbyServer());
    }
}
