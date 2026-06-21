using System.ComponentModel;

namespace Lobby;

public sealed class Translations
{
    [Description("Placeholders: {username}, {user_id}")]
    public string DefaultText { get; set; } = "<size=5>Hello {username}</size>";

    [Description("Placeholders: {server}, {server_name}, {online}, {max_players}, {queue_count}")]
    public string DefaultPortalText { get; set; } =
        "<size=5>{server}\n{online}/{max_players} {queue_count}</size>";

    [Description("No placeholders.")]
    public string UnknownPortal { get; set; } = "Unknown";
}
