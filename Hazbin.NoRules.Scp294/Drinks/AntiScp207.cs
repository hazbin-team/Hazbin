using Exiled.API.Enums;
using Exiled.API.Features;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class AntiScp207 : Drink {
    public override string Name => "Анти Scp 207";
    public override string Description => "Шугар фри!";
    public override int Limit => 10;

    protected override Hint Hint => new() {
        Content = "Я сила!"
    };

    public override Effect[] Effects => [
        new(EffectType.AntiScp207, 150.0f)
    ];
}