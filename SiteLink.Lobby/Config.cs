using Lobby.Models;
using SiteLink.API.Models;

namespace Lobby;

public class Config
{
    public string LobbySchematicFile { get; set; } = "";

    public VectorInfo SpawnLocation { get; set; } = new VectorInfo()
    {
        Y = -299f,
    };

    public List<TextInfo> Texts { get; set; } = new List<TextInfo>()
    {
        new TextInfo()
        {
            PositionX = 0f,
            PositionY = -298f,
            PositionZ = 3.3f,
        }
    };

    public List<PortalInfo> Portals { get; set; } = new List<PortalInfo>()
    {
        new PortalInfo()
        {
            TargetServer = "server1",
            PositionX = 0f,
            PositionY = -298f,
            PositionZ = -4f,
            Rotation = 180f,
        }
    };
}