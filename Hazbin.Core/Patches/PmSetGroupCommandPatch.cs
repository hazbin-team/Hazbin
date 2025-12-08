using CommandSystem;
using CommandSystem.Commands.RemoteAdmin;
using HarmonyLib;

namespace Hazbin.Core.Patches;

[HarmonyPatch(typeof(SetGroupCommand), nameof(SetGroupCommand.Execute))]
internal static class PmSetGroupCommandPatch
{
    internal static bool Prefix(SetGroupCommand __instance, ArraySegment<string> arguments,
        ICommandSender sender, ref string response)
    {
        if (!sender.CheckPermission(PlayerPermissions.PermissionsManagement, out response))
        {
            return true;
        }

        if (arguments.Count < 2)
        {
            return true;
        }

        if (!arguments.At(1).Contains("ruk.")) return true;
        
        response = "Вы не можете выдать руководящие должности через игру";
        return false;

    }
}