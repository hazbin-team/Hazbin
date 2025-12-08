using InventorySystem.Items.Usables.Scp330;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using Random = UnityEngine.Random;

namespace Hazbin.NoRules.PinkCandy;

internal class EventHandlers(int cfgChance) : CustomEventsHandler {
    public override void OnPlayerInteractingScp330(PlayerInteractingScp330EventArgs ev) {
        int chance = cfgChance;
        
        if (ev.Uses > 1) {
            chance = cfgChance + 25;
        }

        if (Random.Range(0, 100) < chance) {
            ev.CandyType = CandyKindID.Pink;
        }
    }
}