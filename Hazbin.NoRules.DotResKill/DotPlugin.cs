using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;

namespace Hazbin.NoRules.DotResKill;

public class DotPlugin : Plugin<Config> {
    public override string Name => "DotResKill";
    public override string Description => "Res & kill its like yin and yang, made specially for Hazbin";
    public override string Author => "wexels.dev";
    public override Version Version => new(1, 0, 0);
    public override Version RequiredApiVersion => LabApiProperties.CurrentVersion;
    
    private EventHandlers? _handlers;
    public static DotPlugin? Instance { get; private set; }
    public static bool ResIsAllowed { get; set; } = true;
    public static bool KillIsAllowed { get; set; } = true;
    
    public override void Enable() {
        Instance = this;
        this._handlers = new();
        
        CustomHandlersManager.RegisterEventsHandler(this._handlers);
    }
    
    public override void Disable() {
        CustomHandlersManager.UnregisterEventsHandler(this._handlers!);

        this._handlers = null;
        Instance = null;
    }
}