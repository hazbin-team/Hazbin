using LabApi.Events.CustomHandlers;
using LabApi.Loader.Features.Plugins;

namespace Hazbin.Teleports;

public class TeleportPlugin : Plugin {
    public override string Name => "Hazbin.Teleports";
    public override string Description => "Easy to use api for teleportation";
    public override string Author => "wexels.dev && NotAloneAgain";
    public override Version Version => new(6, 0, 0);
    public override Version RequiredApiVersion => LabApi.Features.LabApiProperties.CurrentVersion;

    private EventHandlers? _handlers;
    
    public override void Enable() {
        this._handlers = new EventHandlers();
        
        CustomHandlersManager.RegisterEventsHandler(this._handlers);
    }
    
    public override void Disable() {
        CustomHandlersManager.UnregisterEventsHandler(this._handlers!);

        this._handlers = null;
    }
}