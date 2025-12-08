using Exiled.API.Features;
using Hazbin.NoRules.Scp294.EventArgs;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Meat : Drink
{
    public override string Name => "Жидкое мясо";
    public override string Description => "Говядина в бутылке";
    public override int Limit => 6;

    protected override Hint Hint => new()
    {
        Content = "Уххххх, на вкус не очень"
    };
        
    protected override void OnDrinked(DrinkedEventArgs ev)
    {
        ev.Player.Health = 50f;
    }
}