using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;

namespace Hazbin.NoRules.FriendlyFire;

public class FfPlugin : Plugin<Config> {
    public override string Name => "Friendly Fire";
    public override string Description => "KILL YOUR FRIENDS, made specially for Hazbin";
    public override string Author => "wexels.dev";
    public override Version Version => new(1, 0, 0);
    public override Version RequiredApiVersion => LabApiProperties.CurrentVersion;
    
    private EventHandlers? _handlers;
    
    public override void Enable() {
        this._handlers = new EventHandlers(this.Config!.Message);
        
        CustomHandlersManager.RegisterEventsHandler(this._handlers);
    }
    
    public override void Disable() {
        CustomHandlersManager.UnregisterEventsHandler(this._handlers!);
        
        this._handlers = null;
    }
}