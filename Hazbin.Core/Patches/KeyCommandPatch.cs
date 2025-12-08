using CommandSystem;
using CommandSystem.Commands.RemoteAdmin;
using HarmonyLib;

namespace Hazbin.Core.Patches;

[HarmonyPatch(typeof(KeyCommand), nameof(KeyCommand.Execute))]
internal static class KeyCommandPatch {
    internal static bool Prefix(KeyCommand __instance, ArraySegment<string> arguments,
        ICommandSender sender, ref string response) {
        response = "Данная команда отключена в целях безопасности";
        return false;
    }
}