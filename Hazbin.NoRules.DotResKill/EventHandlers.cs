using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using PlayerRoles;

namespace Hazbin.NoRules.DotResKill;

internal class EventHandlers : CustomEventsHandler {
    public override void OnPlayerDeath(PlayerDeathEventArgs ev) {
        if (DotPlugin.ResIsAllowed && Round.Duration.TotalSeconds <= 180) {
            ev.Player.ClearBroadcasts();
            ev.Player.SendBroadcast("\nВы можете заспавнится написав '.res' в консоль на [~]", (ushort)(180 - Round.Duration.TotalSeconds));
        }
    }

    public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev) {
        if (ev.NewRole is RoleTypeId.None or RoleTypeId.Spectator or RoleTypeId.Destroyed) return;
        
        ev.Player.ClearBroadcasts();
    }

    public override void OnServerWaitingForPlayers() {
        DotPlugin.ResIsAllowed = false;
        DotPlugin.KillIsAllowed = false;
    }

    public override void OnServerRoundStarted() {
        DotPlugin.ResIsAllowed = true;
        DotPlugin.KillIsAllowed = true;
    }
}