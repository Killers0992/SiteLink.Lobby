using Lobby.Models;
using PlayerRoles;
using Portals.Core;
using SiteLink.API.Core;
using SiteLink.API.Enums;
using SiteLink.API.Misc;
using SiteLink.API.Networking;
using SiteLink.API.Networking.Common;
using SiteLink.API.Networking.Objects;
using SiteLink.API.Translations;
using UnityEngine;

namespace Lobby.Worlds;

public class LobbyWorld : World
{
    public ConfigSynchronizerObject ConfigSync;
    private DateTime _nextPositionUpdate;
    private DateTime _nextLocalizedTextUpdate;
    private readonly List<(TextToyObject Object, string Template)> _localizedTexts = new();

    public static Quaternion GetQuaternionFromEuler(Vector3 eulerDegrees)
    {
        float x = eulerDegrees.x * Mathf.Deg2Rad * 0.5f;
        float y = eulerDegrees.y * Mathf.Deg2Rad * 0.5f;
        float z = eulerDegrees.z * Mathf.Deg2Rad * 0.5f;

        float sinX = Mathf.Sin(x);
        float cosX = Mathf.Cos(x);

        float sinY = Mathf.Sin(y);
        float cosY = Mathf.Cos(y);

        float sinZ = Mathf.Sin(z);
        float cosZ = Mathf.Cos(z);

        Quaternion q;

        q.x = sinX * cosY * cosZ + cosX * sinY * sinZ;
        q.y = cosX * sinY * cosZ - sinX * cosY * sinZ;
        q.z = cosX * cosY * sinZ - sinX * sinY * cosZ;
        q.w = cosX * cosY * cosZ + sinX * sinY * sinZ;

        return Normalize(q);
    }

    public static Quaternion GetQuaternionFromEuler(float x, float y, float z)
    {
        return GetQuaternionFromEuler(new Vector3(x, y, z));
    }

    private static Quaternion Normalize(Quaternion q)
    {
        float length = Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);

        if (length < Mathf.Epsilon)
            return Quaternion.identity;

        float inverse = 1f / length;

        return new Quaternion(
            q.x * inverse,
            q.y * inverse,
            q.z * inverse,
            q.w * inverse
        );
    }

    public LobbyWorld() : base("Lobby")
    {
        DestroyOnEmpty = false;

        AddWaypoint(MainClass.Singleton.Config.SpawnLocation.ToVector());

        ConfigSync = new ConfigSynchronizerObject(this);
        ConfigSync.ServerConfigSynchronizer.ServerName = "Proxy";

        foreach (TextInfo text in MainClass.Singleton.Config.Texts)
        {
            TextToyObject textObject = new TextToyObject(this);

            textObject.TextToy.Position = new Vector3(text.PositionX, text.PositionY, text.PositionZ);
            textObject.TextToy.Scale = Vector3.one;

            string template = string.IsNullOrWhiteSpace(text.Text)
                ? MainClass.Singleton.Translation.DefaultText
                : text.Text;
            textObject.TextToy.TextFormat = string.Empty;
            textObject.TextToy.DisplaySize = new Vector2(150f, 25f);
            _localizedTexts.Add((textObject, template));
        }

        foreach (PortalInfo portal in MainClass.Singleton.Config.Portals)
        {
            new Portal(
                this,
                portal.TargetServer,
                () => string.IsNullOrWhiteSpace(portal.Text)
                    ? MainClass.Singleton.Translation.DefaultPortalText
                    : MainClass.GetPortalTextByServer(portal.TargetServer),
                new Vector3(portal.PositionX, portal.PositionY, portal.PositionZ),
                GetQuaternionFromEuler(0f, portal.Rotation, 0f));
        }

        if (Schematic.LoadFromFile(MainClass.Singleton.Config.LobbySchematicFile, out Schematic schematic))
        {
            schematic.Load(this, MainClass.Singleton.Config.SpawnLocation);
        }
    }

    public override void Update()
    {
        PortalController.Update(this);

        if (_nextLocalizedTextUpdate <= DateTime.UtcNow)
        {
            foreach (Session observer in GetClientsSnapshot())
            {
                foreach ((TextToyObject text, string template) in _localizedTexts)
                    text.SendText(observer, template, TranslationContext.For(observer, plugin: MainClass.Singleton));
            }

            _nextLocalizedTextUpdate = DateTime.UtcNow.AddSeconds(1);
        }

        if (_nextPositionUpdate > DateTime.UtcNow)
            return;

        SendFpcPositions();
        _nextPositionUpdate = DateTime.UtcNow.AddMilliseconds(10);
    }

    public override void OnLoad(Session session)
    {
        if (session.Player == null)
            session.SpawnPlayer(MainClass.Singleton.Config.SpawnLocation.ToVector());

        session.Player.PlayerAuthenticationManager.SyncedUserId = session.UserId;
    }

    public override void OnObjectsSpawned(Session session)
    {
        session.Connection.AsServer.Seed(350);

        Session[] players = GetClientsSnapshot()
            .Where(player => player.Player != null)
            .ToArray();

        foreach (Session player in players)
        {
            session.Connection.AsServer.Role(player.NetworkId, RoleTypeId.Tutorial);
            session.Connection.AsServer.Health(player.NetworkId, 100f);
        }

        foreach (Session observer in players)
        {
            if (observer == session || observer.Connection == null)
                continue;

            observer.Connection.AsServer.Role(session.NetworkId, RoleTypeId.Tutorial);
            observer.Connection.AsServer.Health(session.NetworkId, 100f);
        }

        session.Connection.AsServer.Stamina(session.NetworkId, 100f);
    }

    public override void OnDestroy()
    {
        Portal.SpawnedPortals.Remove(this);
    }
}
