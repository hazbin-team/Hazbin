using Exiled.API.Features;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Cola : Drink {
    public override string Name => "Кола";
    public override string Description => "Санкционочка";
    public override int Limit => 10;

    protected override Hint Hint => new() {
        Content = "Трамп(не сосал) исправь мои проблемы"
    };
}