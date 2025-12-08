using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;

namespace Hazbin.NoRules.PinkCandy;

public class PinkPlugin : Plugin<Config> {
    public override string Name => "PinkCandy";
    public override string Description => "Explode yourself, made specially for Hazbin";
    public override string Author => "wexels.dev";
    public override Version Version => new(1, 0, 0);
    public override Version RequiredApiVersion => LabApiProperties.CurrentVersion;
    
    private EventHandlers? _handlers;
    
    public override void Enable() {
        this._handlers = new(this.Config!.PinkCandyChance);
        
        CustomHandlersManager.RegisterEventsHandler(this._handlers);
    }
    
    public override void Disable() {
        CustomHandlersManager.UnregisterEventsHandler(this._handlers!);

        this._handlers = null;
    }
}