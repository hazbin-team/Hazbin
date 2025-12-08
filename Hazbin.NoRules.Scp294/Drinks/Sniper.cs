using Exiled.API.Enums;
using Exiled.API.Features;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Sniper : Drink
{
    public override string Name => "Снайпер";
    public override string Description => "Бум, в голову";
    public override int Limit => 4;

    protected override Hint Hint => new()
    {
        Content = "Да! Да нет, пап, я не чокнутый стрелок, я убийца!.. \nРазница в том, что первое — болезнь, а второе — профессия!",
        Duration = 5.0f
    };
    public override Effect[] Effects =>
    [
        new(EffectType.Scp1853, 0f, 255)
    ];
}