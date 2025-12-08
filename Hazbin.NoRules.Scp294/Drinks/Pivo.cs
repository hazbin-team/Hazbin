using Exiled.API.Enums;
using Exiled.API.Features;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Pivo : Drink
{
    public override string Name => "Балтика 7";
    public override string Description => "Легендарный сорт";
    public override int Limit => 11;

    public override Effect[] Effects =>
    [
        new(EffectType.Invigorated, 5f)
    ];

    protected override Hint Hint => new()
    {
        Content = "Теперь я скуф"
    };
}