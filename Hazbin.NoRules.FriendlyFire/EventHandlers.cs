using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;

namespace Hazbin.NoRules.FriendlyFire;

internal class EventHandlers(string message) : CustomEventsHandler {
    public override void OnServerRoundEnded(RoundEndedEventArgs ev) {
        Server.FriendlyFire = true;
        foreach (Player player in Player.List) {
            player.SendBroadcast(message, 60, shouldClearPrevious: true);
        }
    }

    internal void OnRoundStart() => Server.FriendlyFire = false;
}