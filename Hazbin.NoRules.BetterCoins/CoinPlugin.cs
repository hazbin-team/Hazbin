using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;

namespace Hazbin.NoRules.BetterCoins;

public class CoinPlugin : Plugin<Config> {
    public override string Name => "BetterCoins";
    public override string Description => "Flip coin to do something, made specially for Hazbin";
    public override string Author => "wexels.dev";
    public override Version Version => new(1, 0, 0);
    public override Version RequiredApiVersion => LabApiProperties.CurrentVersion;
    
    private EventHandlers? _handlers;
    public static CoinPlugin? Instance;
    
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