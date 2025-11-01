![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/Killers0992/SiteLink/total?label=Downloads&labelColor=2e343e&color=00FFFF&style=for-the-badge)
[![Discord](https://img.shields.io/discord/1216429195232673964?label=Discord&labelColor=2e343e&color=00FFFF&style=for-the-badge)](https://discord.gg/czQCAsDMHa)
# SiteLink.Lobby

**SiteLink.Lobby** is a plugin for [SiteLink](https://github.com/Killers0992/SiteLink) that adds a fully functional **lobby system** to your SCP: Secret Laboratory proxy network.  
It creates a dedicated lobby world where players can spawn, interact with floating texts, and use portals to connect to other SiteLink-managed servers.

---

## 🧩 Requirements

To use **SiteLink.Lobby**, the following are required:

| Dependency | Version |
|-------------|----------|
| [SiteLink](https://github.com/Killers0992/SiteLink) | **0.0.1** or newer |
| [SiteLink.Portals](https://github.com/Killers0992/SiteLink.Portals) | **1.0.0** |

Make sure both are installed and working before adding this plugin.

---

## ✨ Features

- **Dedicated Lobby World** – A lightweight scene where players spawn before joining servers.  
- **Interactive Portals** – Each portal connects to a configured SiteLink server.  
- **Customizable Texts** – Add floating 3D text (server names, info boards, etc.) anywhere in the lobby.  
- **Automatic Player Handling** – Spawns, roles, and transitions between servers handled automatically.  
- **Easy Configuration** – Simple YAML configuration for texts and portals.  

---

## 🚀 How It Works

- Players connect to the Lobby server managed by SiteLink.
- The Lobby World is instantiated, spawning all text and portal objects.
- Each Portal leads to a configured SiteLink target server.
- When a player enters a portal, they are seamlessly transferred to the destination server.

## 📦 Installation

- Place the compiled ``Lobby.dll`` into your SiteLink ``Plugins`` directory.
- Start SiteLink once to generate the default ``config.yml`` file.
- Adjust portal and text coordinates as needed.

## SiteLink Configuration

- Open your SiteLink settings ``settings.yml``

- ✅ ``lobby`` must appear in both:
```
 - listeners[0].priorities → to make it the default connection target.
 - servers_in_selector → to make it visible in the in-game Server Selector menu.
```

- If you want players to return to the ``lobby`` via in-game ``Server Specific Settings``,
simply include ``lobby`` in the ``servers_in_selector`` list.

- The order of entries in priorities defines fallback routing — if lobby is offline, players are sent to the next server.
