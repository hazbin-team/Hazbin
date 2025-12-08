using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;

namespace Hazbin.NoRules.FacilityAuth;

internal class EventHandlers : CustomEventsHandler
{
    public override void OnPlayerTriggeringTesla(PlayerTriggeringTeslaEventArgs ev) {
        if (ev.Player.Items.Any(x => 
                x.Category == ItemCategory.Keycard && 
                x.Type != ItemType.KeycardChaosInsurgency)) {
            ev.IsAllowed = false;
        }
    }
}