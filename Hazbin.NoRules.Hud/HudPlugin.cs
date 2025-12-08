using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using MEC;

namespace Hazbin.NoRules.Hud;

public class HudPlugin : Plugin<Config> {
    public override string Name => "Hud";
    public override string Description => "Good hud, made specially for Hazbin";
    public override string Author => "wexels.dev";
    public override Version Version => new(1, 0, 0);
    public override Version RequiredApiVersion => LabApiProperties.CurrentVersion;

    public static HudPlugin? Instance;
    public EventHandlers? _handlers;
    
    public override void Enable() {
        Instance = this;
        this._handlers = new();
        
        CustomHandlersManager.RegisterEventsHandler(this._handlers);
        
        Timing.RunCoroutine(EventHandlers.ShowRoundTime(), "roundTime");
        Timing.RunCoroutine(EventHandlers.UpdateServerNamePos(), "serverNamePos");
        Timing.RunCoroutine(EventHandlers.ShowPlayerInfo(), "playerInfo");
        Timing.RunCoroutine(EventHandlers.UpdateScpInfo(), "updateScpInfo");
        Timing.RunCoroutine(EventHandlers.SpecInfoSelector(), "specInfoSelector");
        Timing.RunCoroutine(EventHandlers.UpdateRespawnTimer(), "updateRespawnTimer");
    }

    public override void Disable() {
        EventHandlers.LocalHints.Clear();
        
        Timing.KillCoroutines("roundTime");
        Timing.KillCoroutines("serverNamePos");
        Timing.KillCoroutines("playerInfo");
        Timing.KillCoroutines("updateScpInfo");
        Timing.KillCoroutines("updateRespawnTimer");
        Timing.KillCoroutines("specInfoSelector");
        
        CustomHandlersManager.UnregisterEventsHandler(this._handlers!);

        this._handlers = null;
        Instance = null;
    }
}