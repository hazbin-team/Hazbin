using Exiled.API.Enums;
using Exiled.API.Features;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Tyagi : Drink
{
    public override string Name => "Бархатные тяги";
    public override string Description => "На пару минут ваша обувь становится бархатной";
    public override int Limit => 4;
    public override string AudioClip => "tyagi";

    protected override Hint Hint => new()
    {
        Content = "<b>Кефтеме</b>"
    };

    public override Effect[] Effects =>
    [
        new()
        {
            Type = EffectType.SilentWalk,
            Intensity = 100,
            Duration = 90,
            IsEnabled = true
        }
    ];
}