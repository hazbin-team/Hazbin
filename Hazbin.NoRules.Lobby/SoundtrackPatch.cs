using FacilitySoundtrack;
using HarmonyLib;
using LabApi.Features.Wrappers;

namespace Hazbin.NoRules.Lobby;

[HarmonyPatch(typeof(SoundtrackManager), nameof(SoundtrackManager.Update))]
internal static class SoundtrackPatch {
    internal static bool Prefix() => Round.IsRoundInProgress;
}