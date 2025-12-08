using Exiled.API.Enums;
using Exiled.API.Features;
using Hazbin.NoRules.Scp294.EventArgs;
using Hazbin.NoRules.Scp294.Models;
using UnityEngine;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Baikal : Drink {
    public override string Name => "Байкал";
    public override string Description => "Напиток древних русов";
    public override int Limit => 8;
    
    public override Effect[] Effects => [
        new(EffectType.Slowness, 20f, 20)
    ];
    protected override Hint Hint => new() {
        Content = "Теперь ты славянин"
    };
        
    protected override void OnDrinked(DrinkedEventArgs ev) {
        ev.Player.Scale = Vector3.one * 1.25f;
    }
}