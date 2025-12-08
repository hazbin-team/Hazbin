using Hazbin.Core.Extensions;
using HintServiceMeow.Core.Enum;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using Respawning.Objectives;

namespace Hazbin.NoRules.ShowReports;

internal class EventHandlers : CustomEventsHandler {
    public override void OnPlayerReportedPlayer(PlayerReportedPlayerEventArgs ev) => this.SendReport(ev.Player, ev.Target);
    public override void OnPlayerReportedCheater(PlayerReportedCheaterEventArgs ev) => this.SendReport(ev.Player, ev.Target);

    private void SendReport(Player reporter, Player target) {
        if (target.UserId == reporter.UserId) return; 
        
        foreach (Player? player in Player.List.Where(x => x.UserGroup != null && PermissionsHandler.IsPermitted(x.UserGroup.Permissions, PlayerPermissions.AdminChat))) {
            player.ClearHints("report");
            player.ShowHint($"<b>[<color=yellow>Репорт</color> на <color={target.Role.GetRoleColor().ToHex()}>{target.DisplayName}</color>]\nПроверьте Staff Chat (M)</b>", 
                new(0, 900), 10.0f, HintVerticalAlign.Middle, fontSize: 32, tag: "report");
        }

        reporter.SendBroadcast($"<b><color=yellow>Репорт</color> на {target.DisplayName} отправлен <color=green>админам и в дискорд</color>!</b>", 15);
    }
}