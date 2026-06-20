using Lobby.Models;
using PlayerRoles;
using Portals.Core;
using SiteLink.API.Core;
using SiteLink.API.Enums;
using SiteLink.API.Misc;
using SiteLink.API.Networking;
using SiteLink.API.Networking.Common;
using SiteLink.API.Networking.Objects;
using UnityEngine;

namespace Lobby.Worlds;

public class LobbyWorld : World
{
    public ConfigSynchronizerObject ConfigSync;

    public LobbyWorld() : base("Lobby")
    {
        DestroyOnEmpty = true;

        AddWaypoint(MainClass.Singleton.Config.SpawnLocation.ToVector());

        ConfigSync = new ConfigSynchronizerObject(this);
        ConfigSync.ServerConfigSynchronizer.ServerName = "Proxy";

        foreach (TextInfo text in MainClass.Singleton.Config.Texts)
        {
            TextToyObject textObject = new TextToyObject(this);

            textObject.TextToy.Position = new Vector3(text.PositionX, text.PositionY, text.PositionZ);
            textObject.TextToy.Scale = Vector3.one;

            textObject.TextToy.TextFormat = text.Text;
            textObject.TextToy.DisplaySize = new Vector2(150f, 25f);
        }

        foreach (PortalInfo portal in MainClass.Singleton.Config.Portals)
        {
            new Portal(this, portal.TargetServer, portal.Text, new Vector3(portal.PositionX, portal.PositionY, portal.PositionZ), new Quaternion(0f, portal.Rotation, 0f, 0f));
        }

        if (Schematic.LoadFromFile(MainClass.Singleton.Config.LobbySchematicFile, out Schematic schematic))
        {
            schematic.Load(this, MainClass.Singleton.Config.SpawnLocation);
        }
    }

    public override void Update()
    {
        PortalController.Update(this);
    }

    public override void OnLoad(Session session)
    {
        session.SpawnPlayer(MainClass.Singleton.Config.SpawnLocation.ToVector());
    }

    public override void OnObjectsSpawned(Session session)
    {
        session.Connection.AsServer.Seed(350);

        session.Connection.AsServer.Role(session.NetworkId, RoleTypeId.Tutorial);
        session.Connection.AsServer.Health(session.NetworkId, 100f);
        session.Connection.AsServer.Stamina(session.NetworkId, 100f);

        session.Player.PlayerAuthenticationManager.SyncedUserId = session.UserId;

        //ConfigSync.SendUpdate(session);
    }

    public override void OnDestroy()
    {
        Portal.SpawnedPortals.Remove(this);
    }
}
