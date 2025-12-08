using Exiled.API.Features;
using Hazbin.NoRules.Scp294.EventArgs;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Master : Drink
{
    public override string Name => "Dungeon master";
    public override string Description => "напиток качков";
    public override int Limit => 5;

    protected override Hint Hint => new()
    {
        Content = "вас научили мужской дружбе"
    };
        
    protected override void OnDrinked(DrinkedEventArgs ev)
    {
        ev.Player.Health = 10f;
    }
}