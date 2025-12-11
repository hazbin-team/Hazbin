using Hazbin.NoRules.PlayerXp.Models;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Paths;
using LabApi.Loader.Features.Plugins;

namespace Hazbin.NoRules.PlayerXp;

public class XpPlugin : Plugin {
    public override string Name => "PlayerXp";
    public override string Description => "spam doors, made specially for Hazbin";
    public override string Author => "wexels.dev";
    public override Version Version => new(1, 0, 0);
    public override Version RequiredApiVersion => LabApiProperties.CurrentVersion;
    
    public static XpPlugin Instance { get; private set; }
    private EventHandlers? _handlers;
    internal Database? _database;
    
    public override void Enable() {
        Instance = this;
        this._handlers = new();
        this._database = new(Path.Combine(PathManager.Configs.FullName, Server.Port.ToString(), this.Name));
        
        CustomHandlersManager.RegisterEventsHandler(this._handlers);
    }
    
    public override void Disable() {
        CustomHandlersManager.UnregisterEventsHandler(this._handlers!);

        this._database!.Dispose();
        this._handlers = null;
        Instance = null;
    }
}