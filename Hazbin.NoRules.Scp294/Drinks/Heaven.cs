using Exiled.API.Features;
using Hazbin.Core.Extensions;
using Hazbin.NoRules.Scp294.EventArgs;
using Hazbin.NoRules.Scp294.Models;
using MEC;

namespace Hazbin.NoRules.Scp294.Drinks;

public class Heaven : Drink {
    public override string Name => "Агент рая";
    public override string Description => "За тобой уже выехали";
    public override int Limit => 3;

    protected override Hint Hint => new() {
        Content = "Самоуничтожение через 5 секунд."
    };
        
    protected override void OnDrinked(DrinkedEventArgs ev) {
        Timing.CallDelayed(5f, () => {
            ev.Player.ShowCoreHint("Миссия выполнена");
            Player.Get(ev.Player).Explode();
        });
    }
}