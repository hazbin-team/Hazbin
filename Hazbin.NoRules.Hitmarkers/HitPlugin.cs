using Exiled.Events.Handlers;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;

namespace Hazbin.NoRules.Hitmarkers;

public class HitPlugin : Plugin {
    public override string Name => "Hitmarkers";
    public override string Description => "Great hit markers, made specially for Hazbin";
    public override string Author => "wexels.dev";
    public override Version Version => new(1, 0, 0);
    public override Version RequiredApiVersion => LabApiProperties.CurrentVersion;
    
    private EventHandlers? _handlers;
    
    public override void Enable() {
        _handlers = new EventHandlers();
        
        CustomHandlersManager.RegisterEventsHandler(this._handlers);
        
        Player.Hurting += _handlers.OnPlayerHurting;
    }
    
    public override void Disable() {
        Player.Hurting += _handlers!.OnPlayerHurting;
        
        CustomHandlersManager.UnregisterEventsHandler(this._handlers);
        
        _handlers = null;
    }
}