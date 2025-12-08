using Exiled.API.Features;
using Hazbin.Core.Extensions;
using Hazbin.NoRules.Scp294.EventArgs;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Doping : Drink {
    public override string Name => "Трамбалон";
    public override string Description => "Только для сигм";
    public override int Limit => 5;

    protected override Hint Hint => new() {
        Content = "Чтобы стать большим большим качком"
    };
        
    protected override void OnDrinked(DrinkedEventArgs ev) {
        ev.Player.AddAhp(65, persistant: true);
    }
}