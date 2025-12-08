using CommandSystem;
using CommandSystem.Commands.Shared;
using HarmonyLib;

namespace Hazbin.Core.Patches;

[HarmonyPatch(typeof(HelpCommand), "Execute")]
internal static class HelpCommandPatch {
    internal static bool Prefix(HelpCommand __instance, ArraySegment<string> arguments, ICommandSender sender, ref string response) {
        if (!CorePlugin.Instance!.Config.DisableHelp)
            return true;
        
        if (!Player.TryGet(sender, out Player player)) {
            response = "Вы должны быть игроком для выполнения данной команды";
            return false;
        }

        response = player.UserId == "76561198912363436@steam"
            ? UnityEngine.Random.Range(0, 100) <= 5 ? "Банан иди нахуй, ты заебал спамить" : "Отказано в доступе"
            : "Отказано в доступе";
        return false;
    }
}