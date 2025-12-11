using Hazbin.Core.Enums;
using Hazbin.Core.Extensions;
using Hazbin.NoRules.Scp294.EventArgs;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;
using Hint = Exiled.API.Features.Hint;
using Effect = Exiled.API.Features.Effect;

namespace Hazbin.NoRules.Scp294.Models;

public abstract class Drink {
    private HashSet<int> TrackedSerials { get; } = new();

    public int Id = -1;
    
    public abstract string Name { get; }
    public abstract string Description { get; }
    public virtual int Limit { get; } = 15;
    public virtual string? AudioClip { get; } = "";
    protected abstract Hint Hint { get; }
    public virtual Effect[]? Effects { get; } = [];

    internal void Drinking(DrinkingEventArgs ev) => this.OnDrinking(ev);
        
    internal void Drinked(DrinkedEventArgs ev) {
        if (this.Effects != null) {
            foreach (Effect effect in this.Effects) {
                ev.Player.EnableEffect((EffectType)effect.Type, effect.Intensity, effect.Duration, effect.AddDurationIfActive);
            }
        }

        if (this.Hint.Show) {
            ev.Player.ShowCoreHint(this.Hint.Content, this.Hint.Duration);
        }

        if (!string.IsNullOrEmpty(this.AudioClip)) {
            AudioPlayer drinkPlayer = AudioPlayer.CreateOrGet($"{ev.Player.Nickname}_{ev.Drink.Name}",
                onIntialCreation: p => {
                    Speaker speaker = p.AddSpeaker("Main", maxDistance: 10.0f);

                    speaker.Volume = 1.0f;
                    p.transform.SetParent(ev.Player.GameObject!.transform);
                    speaker.transform.SetParent(ev.Player.GameObject.transform);
                    speaker.transform.localPosition = Vector3.up;
                });

            drinkPlayer.AddClip(this.AudioClip);
            Logger.Debug($"{ev.Drink.Name}: AudioClip {this.AudioClip} has been played");
        }

        this.OnDrinked(ev);
    }
        
    protected virtual void OnDrinking(DrinkingEventArgs ev) { }
    protected virtual void OnDrinked(DrinkedEventArgs ev) { }
    
    protected internal virtual void OnPickedUpItem(PlayerPickedUpItemEventArgs ev) {
        ev.Player.ShowCoreHint($"Вы подобрали <b>{this.Name}</b> \n{this.Description}");
    }

    protected internal virtual void OnChangedItem(PlayerChangedItemEventArgs ev) {
        ev.Player.ShowCoreHint($"Вы держите <b>{this.Name}</b> \n{this.Description}");
    }
    
    public virtual bool Check(Pickup? pickup) => pickup != null && this.TrackedSerials.Contains(pickup.Serial);
    public virtual bool Check(Item? item) => item != null && this.TrackedSerials.Contains(item.Serial);
    
    public virtual void Give(Player player) {
        try {
            Item? item = player.AddItem(ItemType.AntiSCP207);

            Logger.Debug($"{nameof(this.Give)}: Adding {item.Serial} to tracker.");
            this.TrackedSerials.Add(item.Serial);

            player.CurrentItem = item;
        }
        catch (Exception e) {
            Logger.Error($"{nameof(this.Give)}: {e}");
        }
    }

    public void Register() {
        Scp294Plugin.Instance!._drinkMachine!.AddDrink(this);
        this.Id = Scp294Plugin.Instance._drinkMachine.GetDrinks().IndexOf(this);
    }
}