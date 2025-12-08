using LabApi.Features.Wrappers;

namespace Hazbin.NoRules.BetterCoins;

internal static class Extensions {
    private static readonly HashSet<Player> Teleporting;

    static Extensions() {
        Teleporting = new(Server.MaxPlayers);
    }

    internal static void AddToTeleporting(this Player player) => Teleporting.Add(player);
    internal static void RemoveFromTeleporting(this Player player) => Teleporting.Remove(player);
    internal static bool IsTeleporting(this Player player) => Teleporting.Contains(player);
}