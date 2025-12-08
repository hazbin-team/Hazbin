using Exiled.API.Features;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Water : Drink
{
    public override string Name => "Вода";
    public override string Description => "На вид как вода";
    public override int Limit => 5;

    protected override Hint Hint => new()
    {
        Content = "Вода как вода, ничего необычного"
    };
}