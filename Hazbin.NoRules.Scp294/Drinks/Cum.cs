using Exiled.API.Features;
using Hazbin.NoRules.Scp294.EventArgs;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Cum : Drink {
    public override string Name => "Cum";
    public override string Description => "МММ";
    public override int Limit => 10;

    protected override Hint Hint => new() {
        Content = "Oh shit, i am sorry"
    };
        
    protected override void OnDrinked(DrinkedEventArgs ev) {
        ev.Player.Health -= 10f;
    }
}