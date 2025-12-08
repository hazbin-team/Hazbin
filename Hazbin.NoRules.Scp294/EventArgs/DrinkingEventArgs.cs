using Hazbin.Core.Interfaces;
using Hazbin.NoRules.Scp294.Models;
using LabApi.Features.Wrappers;

namespace Hazbin.NoRules.Scp294.EventArgs;

public class DrinkingEventArgs(Drink drink, Player player, UsableItem item, bool isAllowed) : IDeniableEvent
{
    public UsableItem Item { get; set; } = item;
    public Drink Drink { get; set; } = drink;
    public Player Player { get; set; } = player;
    public bool IsAllowed { get; set; } = isAllowed;
}