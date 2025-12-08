using Hazbin.NoRules.BetterEscape.Features;
using LabApi.Features.Wrappers;
using PlayerRoles;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

namespace Hazbin.NoRules.BetterEscape.Components;

public sealed class EscapeComponent : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (!Player.TryGet(other.gameObject, out Player player)) {
            Logger.Debug("No player");
            return;
        }

        /*if (SubjectManager.IsSubject(player)) {
            Logger.Debug("player is subject");
            return; 
        }*/
        
        RoleTypeId role = EscapeScenarioManager.GetScenario(player);
        if (role == RoleTypeId.None) {
            Logger.Debug("No scenario");
            return; 
        }

        Logger.Debug("Getting items");
        List<Item> items = player.Items.ToList();
        
        player.SetRole(role, RoleChangeReason.Escaped);
        Logger.Debug("Set role");
        
        Logger.Debug("Dropping items");
        foreach (Item item in items) {
            item.DropItem();
            Logger.Debug("Dropped " + item.Type);
        }
        
        items.Clear();
        
        Logger.Debug($"{player.Nickname} escaped as {role}");
    }
}