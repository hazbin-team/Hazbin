using Exiled.API.Features;
using Hazbin.NoRules.Scp294.Models;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Tea : Drink
{
    public override string Name => "Чай";
    public override string Description => "Горячий на вид";
    public override int Limit => 5;

    protected override Hint Hint => new()
    {
        Content = "Сахар где?!"
    };
}