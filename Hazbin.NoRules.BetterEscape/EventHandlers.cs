using Hazbin.NoRules.BetterEscape.Features;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using PlayerRoles;
using Logger = LabApi.Features.Console.Logger;

namespace Hazbin.NoRules.BetterEscape;

public class EventHandlers : CustomEventsHandler {
    public override void OnPlayerEscaping(PlayerEscapingEventArgs ev) {
        ev.IsAllowed = false;
        
        /*if (SubjectManager.IsSubject(player)) {
            Logger.Debug("player is subject");
            return;
        }*/
        
        RoleTypeId role = EscapeScenarioManager.GetScenario(ev.Player);
        if (role == RoleTypeId.None) {
            Logger.Debug("No scenario");
            return; 
        }

        Logger.Debug("Getting items");
        List<Item> items = ev.Player.Items.ToList();
        
        ev.Player.SetRole(role, RoleChangeReason.Escaped);
        Logger.Debug("Set role");
        
        Logger.Debug("Dropping items");
        foreach (Item item in items) {
            item.DropItem();
            Logger.Debug("Dropped " + item.Type);
        }
        
        items.Clear();
        
        Logger.Debug($"{ev.Player.Nickname} escaped as {role}");
    }
}