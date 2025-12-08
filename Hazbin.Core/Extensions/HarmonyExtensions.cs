using System.Reflection;
using HarmonyLib;
using Hazbin.Core.Attributes;

namespace Hazbin.Core.Extensions;

public static class HarmonyExtensions {
    public static void PatchCategory(this Harmony harmony, string category, Assembly? assembly = null) {
        assembly ??= Assembly.GetCallingAssembly();

        assembly.GetTypes().Where(type => {
                IEnumerable<HarmonyPatchCategory> categories = type.GetCustomAttributes<HarmonyPatchCategory>();
                return categories.Any(c => c.Category == category);
            })
            .Do(type => SafePatch(harmony, type));
    }

    public static void PatchAllNoCategory(this Harmony harmony, Assembly? assembly = null) {
        assembly ??= Assembly.GetCallingAssembly();

        assembly.GetTypes().Where(type => {
                IEnumerable<HarmonyPatchCategory> categories = type.GetCustomAttributes<HarmonyPatchCategory>();
                return !categories.Any();
            })
            .Do(type => SafePatch(harmony, type));
    }

    private static void SafePatch(Harmony harmony, Type type) {
        try {
            harmony.CreateClassProcessor(type).Patch();
        }
        catch (Exception ex) {
            Logger.Error($"[HarmonyExtensions] failed to safely patch {harmony.Id} ({type.FullName}): {ex}");
        }
    }
}