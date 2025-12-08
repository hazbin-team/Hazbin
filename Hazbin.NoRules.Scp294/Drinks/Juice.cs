using Exiled.API.Enums;
using Exiled.API.Features;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Juice : Drink
{
    public override string Name => "Адский сок";
    public override string Description => "Сделан на основе лавы и крови";
    public override int Limit => 7;

    public override Effect[] Effects =>
    [
        new(EffectType.Slowness, 10f, 20)
    ];

    protected override Hint Hint => new()
    {
        Content = "Ох,с дымком"
    };
}