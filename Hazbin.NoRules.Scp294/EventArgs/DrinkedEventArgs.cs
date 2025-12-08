using Hazbin.NoRules.Scp294.Models;
using LabApi.Features.Wrappers;

namespace Hazbin.NoRules.Scp294.EventArgs;

public class DrinkedEventArgs(Drink drink, Player player, UsableItem item)
{
    public Drink Drink { get; set; } = drink;
    public UsableItem Item { get; set; } = item;
    public Player Player { get; set; } = player;
}