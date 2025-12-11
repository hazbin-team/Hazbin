using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using MEC;

namespace Hazbin.NoRules.AutoBroadcast;

internal class EventHandlers(List<Broadcast> broadcasts) : CustomEventsHandler {
    public override void OnPlayerJoined(PlayerJoinedEventArgs ev) {
        ev.Player.SendBroadcast($"<color=#FF1493><b>{ev.Player.Nickname}</b>,</color> <color=#EE82EE><b>приветствуем в царстве ада!</b></color>", 10);
            
        Timing.RunCoroutine(this.BroadcastCoroutine(ev.Player));
    }

    private IEnumerator<float> BroadcastCoroutine(Player player) {
        while (true) {
            Broadcast broadcast = broadcasts.RandomItem();

            yield return Timing.WaitForSeconds(broadcast.Delay);

            player.ClearBroadcasts();
            player.SendBroadcast(broadcast.Message!, broadcast.Duration);
        }
    }
}