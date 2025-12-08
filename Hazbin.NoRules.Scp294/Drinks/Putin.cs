using Exiled.API.Features;
using Hazbin.NoRules.Scp294.EventArgs;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Putin : Drink
{
    public override string Name => "Широкий Путин";
    public override string Description => "Внутри жидкий смокинг";
    public override int Limit => 5;

    protected override Hint Hint => new()
    {
        Content = "Я ваш преZидент"
    };
        
    protected override void OnDrinked(DrinkedEventArgs ev)
    {
        ev.Player.Scale = new(1.25f, 0.9f, 1.25f);
    }
}