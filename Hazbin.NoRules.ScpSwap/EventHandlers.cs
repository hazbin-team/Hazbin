using System.Reflection;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using PlayerRoles;

namespace Hazbin.NoRules.ScpSwap;

internal class EventHandlers : CustomEventsHandler {
    public override void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev) {
        if (ev.NewRole.Team != Team.SCPs) return;
        
        ev.Player.SendBroadcast("\nВы можете изменить свою роль SCP на другую, написав '.swap' и номер в консоль на [~]", (ushort)(90 - Round.Duration.TotalSeconds));
    }

    public override void OnServerWaitingForPlayers() {
        SwapPlugin.IsAllowed = false;
    }

    public override void OnServerRoundStarted() {
        SwapPlugin.IsAllowed = true;
    }
}