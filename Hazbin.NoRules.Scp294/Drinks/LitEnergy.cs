using Exiled.API.Enums;
using Exiled.API.Features;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class LitEnergy : Drink
{
    public override string Name => "Лит Енерджи";
    public override string Description => "Уффф мишаня";
    public override int Limit => 6;

    public override Effect[] Effects =>
    [
        new(EffectType.FogControl, 0f, 10)
    ];

    protected override Hint Hint => new()
    {
        Content = "Вошёл в кондиции"
    };
}