using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using PlayerRoles.Voice;

namespace Hazbin.NoRules.IntercomList;

internal class EventHandlers : CustomEventsHandler {
    private IEnumerator<float> IntercomCoroutine() {
        while (true) {
            string text;
            switch (Intercom.State) {
                case IntercomState.Ready:
                    text = "<size=400>Готово</size>\n";
                    break;
                case IntercomState.Starting:
                    text = "<size=400>Подготовка...</size>\n";
                    break;
                case IntercomState.InUse:
                    text = $"<size=400>Используется, ещё {Math.Round(Intercom._singleton.Network_nextTime, 0)}</size>\n";
                    break;
                case IntercomState.Cooldown:
                    text = $"<size=400>Перезагрузка {Math.Round(Intercom._singleton.RemainingTime, 0)}</size>\n";
                    break;
                default:
                    text = "";
                    break;
            }

            text += "<size=250>";
            text += "<color=white>------------------------</color>\n";
            text += $"<color=orange>Класс Д:</color> <color=white>{Player.List.Count(player => player.Role == RoleTypeId.ClassD)}</color>\n";
            text += $"<color=yellow>Учёные:</color> <color=white>{Player.List.Count(player => player.Role == RoleTypeId.Scientist)}</color>\n";
            int guardCount = Player.List.Count(player => player.Role == RoleTypeId.FacilityGuard);
            text += $"<color=#808080>Охрана:</color> <color=white>{guardCount}</color>\n";
            text += $"<color=blue>МТФ:</color> <color=white>{Player.List.Count(player => player.Team == Team.FoundationForces) - guardCount}</color>\n";
            text += $"<color=green>Хаос:</color> <color=white>{Player.List.Count(player => player.Team == Team.ChaosInsurgency)}</color>\n";
            text += $"<color=red>SCP:</color> <color=white>{Player.List.Count(player => player.Team == Team.SCPs)}</color>\n";
            text += $"<color=#FF1493>Неопределенно:</color> <color=white>{Player.List.Count(player => player.Team == Team.OtherAlive)}</color>\n";
            int spectatorCount = Player.List.Count(player => player.Role is RoleTypeId.Spectator or RoleTypeId.Filmmaker or RoleTypeId.Overwatch);
            text += $"<color=white>Наблюдатели: {spectatorCount}</color>\n";
            text += "<color=white>------------------------</color>\n";
            text += "</size>";

            IntercomDisplay.TrySetDisplay(text);

            yield return Timing.WaitForSeconds(1f);
        }
    }

    public override void OnServerRoundStarted() => Timing.RunCoroutine(this.IntercomCoroutine());
}