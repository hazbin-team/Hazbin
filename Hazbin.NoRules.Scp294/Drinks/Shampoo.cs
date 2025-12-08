using Exiled.API.Enums;
using Exiled.API.Features;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Shampoo : Drink
{
    public override string Name => "Шампунь джумбайсемба";
    public override string Description => "Пахнет аНанАсОм";
    public override int Limit => 7;

    public override Effect[] Effects =>
    [
        new(EffectType.Flashed, 10f)
    ];

    protected override Hint Hint => new()
    {
        Content = "Нахер пить мыло? 0_0"
    };
}