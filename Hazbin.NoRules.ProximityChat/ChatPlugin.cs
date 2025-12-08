using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;

namespace Hazbin.NoRules.ProximityChat;

public class ChatPlugin : Plugin<Config> {
    public override string Name => "ProximityChat";
    public override string Description => "Speak with SCPs, made specially for Hazbin";
    public override string Author => "wexels.dev";
    public override Version Version => new(1, 0, 0);
    public override Version RequiredApiVersion => LabApiProperties.CurrentVersion;
    
    private EventHandlers? _handlers;
    public static ChatPlugin? Instance;
    
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