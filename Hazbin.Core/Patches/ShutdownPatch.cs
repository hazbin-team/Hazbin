using HarmonyLib;

namespace Hazbin.Core.Patches;

[HarmonyPatch(typeof(ServerShutdown), nameof(ServerShutdown.Shutdown))]
internal static class ShutdownPatch {
    internal static void Prefix() {
        foreach (LabApi.Loader.Features.Plugins.Plugin? plugin in LabApi.Loader.PluginLoader.EnabledPlugins) { // nw wtf
            plugin.Disable();
        }
    }
}