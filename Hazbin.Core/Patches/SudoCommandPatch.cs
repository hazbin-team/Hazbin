using CommandSystem;
using CommandSystem.Commands.RemoteAdmin;
using HarmonyLib;

namespace Hazbin.Core.Patches;

[HarmonyPatch(typeof(RconCommand), nameof(RconCommand.Execute))]
internal static class SudoCommandPatch {
    internal static bool Prefix(RconCommand __instance, ArraySegment<string> arguments,
        ICommandSender sender, ref string response) {
        response = "Команда отключена в целях безопасности";
        return false;
    }
}