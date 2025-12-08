using HarmonyLib;
using Hazbin.NoRules.Scp294.Models;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;

namespace Hazbin.NoRules.Scp294;

public class Scp294Plugin : Plugin<Config> {
    public override string Name => "Scp294";
    public override string Description => "Notify admins about reports, made specially for Hazbin";
    public override string Author => "wexels.dev";
    public override Version Version => new(1, 0, 0);
    public override Version RequiredApiVersion => LabApiProperties.CurrentVersion;
    
    private Harmony? harmony;
    private EventHandlers? _handlers;
    public DrinkMachine? _drinkMachine;
    public static Scp294Plugin? Instance;
    
    public override void Enable() {
        Instance = this;
        this.harmony = new Harmony("scp294.hazbin");
        this._drinkMachine = new();
        this._handlers = new(this._drinkMachine);

        this.harmony.PatchAll();
        
        AudioClipStorage.LoadClip(this.Config!.SoundsPath + "drink_machine.ogg", "drink_machine");
        AudioClipStorage.LoadClip(this.Config.SoundsPath + "tyagi.ogg", "tyagi");
        
        CustomHandlersManager.RegisterEventsHandler(this._handlers);
    }
    
    public override void Disable() {
        CustomHandlersManager.UnregisterEventsHandler(this._handlers!);

        this.harmony!.UnpatchAll();
        
        this._handlers = null;
        this._drinkMachine = null;
        this.harmony = null;
        Instance = null;
    }
}