using Exiled.API.Enums;
using Exiled.API.Features;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Scp207 : Drink
{
    public override string Name => "Scp 207";
    public override string Description => "Ням";
    public override int Limit => 8;

    public override Effect[] Effects =>
    [
        new(EffectType.Scp207, 150.0f)
    ];

    protected override Hint Hint => new()
    {
        Content = "Я скорость!"
    };
}