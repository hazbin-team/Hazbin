using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using PlayerRoles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hazbin.NoRules.RandomScale;

internal class EventHandlers : CustomEventsHandler {
    public override void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev) {
        ev.Player.Scale = Vector3.one;
        if (ev.NewRole.RoleTypeId.Equals(RoleTypeId.Tutorial) || ev.NewRole.Team == Team.SCPs) return;
        
        ev.Player.Scale = Vector3.one * Random.Range(0.9f, 1.1f);
    }
}