using Hazbin.Core.Extensions;
using HintServiceMeow.Core.Enum;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using PlayerRoles;
using UnityEngine;

namespace Hazbin.NoRules.ScpSwap;

internal class EventHandlers : CustomEventsHandler {
    public override void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev) {
        if (ev.NewRole.Team != Team.SCPs) return;
        
        ev.Player.ShowHint("<b>Вы можете изменить свою роль SCP на другую, написав '.swap' и номер в консоль на [~]</b>", new Vector2(0, 90), (float)(90 - Round.Duration.TotalSeconds), HintVerticalAlign.Bottom, HintAlignment.Center, 11);
    }

    public override void OnServerWaitingForPlayers() {
        SwapPlugin.IsAllowed = false;
    }

    public override void OnServerRoundStarted() {
        SwapPlugin.IsAllowed = true;
    }
}