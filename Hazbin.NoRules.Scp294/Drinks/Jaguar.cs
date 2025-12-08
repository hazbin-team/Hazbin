using Exiled.API.Enums;
using Exiled.API.Features;
using Hazbin.NoRules.Scp294.EventArgs;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Jaguar : Drink {
    public override string Name => "Ягуар";
    public override string Description => "Верните мой 2007";
    public override int Limit => 4;

    public override Effect[] Effects => [
        new(EffectType.MovementBoost, 10f, 30)
    ];

    protected override Hint Hint => new() {
        Content = "Энергетики вредны для здоровья"
    };
        
    protected override void OnDrinked(DrinkedEventArgs ev) {
        ev.Player.Heal(50f);
    }
}