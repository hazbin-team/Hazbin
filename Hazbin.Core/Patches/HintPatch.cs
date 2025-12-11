using HarmonyLib;
using Hazbin.Core.Extensions;
using Hints;
using Mirror;
using Hint = Hints.Hint;

namespace Hazbin.Core.Patches;

[HarmonyPatch(typeof(HintDisplay), nameof(HintDisplay.Show))]
internal static class HintPatch {
    internal static bool Prefix(HintDisplay __instance, Hint hint) {
        if (__instance.isLocalPlayer)
            throw new InvalidOperationException("Cannot display a hint to the local player (headless server).");
            
        if (!NetworkServer.active)
            return false;
            
        if (HintDisplay.SuppressedReceivers.Contains(__instance.connectionToClient))
            return false;
            
        Player.Get(__instance.netIdentity).ShowCoreHint(hint);
            
        return false;
    }
}