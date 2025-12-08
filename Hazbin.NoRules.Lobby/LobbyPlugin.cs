using HarmonyLib;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;

namespace Hazbin.NoRules.Lobby;

public class LobbyPlugin : Plugin<Config> {
    public override string Name => "Lobby";
    public override string Description => "Lobby, made specially for Hazbin";
    public override string Author => "wexels.dev";
    public override Version Version => new(1, 0, 0);
    public override Version RequiredApiVersion => LabApiProperties.CurrentVersion;
    
    private EventHandlers? _handlers;
    private Harmony? harmony;
    
    public override void Enable() {
        this._handlers = new(this.Config!.SoundsPath);
        this.harmony = new Harmony("hnr.wexels.dev");
        
        CustomHandlersManager.RegisterEventsHandler(this._handlers);

        this.harmony.PatchAll();
    }
    
    public override void Disable() {
        CustomHandlersManager.UnregisterEventsHandler(this._handlers!);

        this.harmony!.UnpatchAll();

        this.harmony = null;
        this._handlers = null;
    }
}