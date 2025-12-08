using HarmonyLib;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;

namespace Hazbin.Core;

public class CorePlugin : Plugin<Config> {
    public override string Name => "Hazbin.Core";
    public override string Description => "Hazbin main lib";
    public override string Author => "wexels.dev";
    public override Version Version => new(1, 0, 0);
    public override Version RequiredApiVersion => LabApiProperties.CurrentVersion;
    
    public static CorePlugin? Instance { get; private set; }
    public Harmony? HarmonyInstance;
    
    public override void Enable() {
        Instance = this;
        if (this.Config!.DisablePatches) {
            this.HarmonyInstance = new Harmony("core.hazbin");
            this.HarmonyInstance.PatchAll();
        }
        
    }

    public override void Disable() {
        this.HarmonyInstance!.UnpatchAll();

        this.HarmonyInstance = null;
        Instance = null;
    }
}