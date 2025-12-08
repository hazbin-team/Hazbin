using Exiled.API.Features;
using Hazbin.NoRules.Scp294.EventArgs;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Vodka : Drink
{
    public override string Name => "Водка";
    public override string Description => "Наше пойло";
    public override int Limit => 5;

    protected override Hint Hint => new()
    {
        Content = "Хорошо пошла!"
    };

    protected override void OnDrinked(DrinkedEventArgs ev)
    {
        ev.Player.Health -= 20f;
    }
}