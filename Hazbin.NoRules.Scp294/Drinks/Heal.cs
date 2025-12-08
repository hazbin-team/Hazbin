using Exiled.API.Features;
using Hazbin.NoRules.Scp294.EventArgs;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Heal : Drink {
    public override string Name => "Хилка";
    public override string Description => "Сделано на основе боярышника";
    public override int Limit => 6;

    protected override Hint Hint => new() {
        Content = "Простатит в прошлом!"
    };
        
    protected override void OnDrinked(DrinkedEventArgs ev) {
        ev.Player.Heal(50f);
    }
}