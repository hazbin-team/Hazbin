using Hazbin.Core.Extensions;
using Hazbin.Teleports.Extensions;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using Random = UnityEngine.Random;

namespace Hazbin.NoRules.BetterCoins;

internal class EventHandlers : CustomEventsHandler {
    public override void OnPlayerChangingItem(PlayerChangingItemEventArgs ev) => ev.IsAllowed = ev.IsAllowed && !ev.Player.IsTeleporting();
    public override void OnPlayerDroppingItem(PlayerDroppingItemEventArgs ev) => ev.IsAllowed = ev.IsAllowed && !ev.Player.IsTeleporting();

    public override void OnPlayerSpawned(PlayerSpawnedEventArgs ev) {
        if (Random.Range(0, 100) > 90 || ev.Player.Role != RoleTypeId.ClassD) 
            return;

        ev.Player.AddItem(ItemType.Coin);
    }

    public override void OnPlayerFlippingCoin(PlayerFlippingCoinEventArgs ev) {
        if (ev.Player.IsHost || 
            ev.Player.IsNpc || 
            !ev.Player.IsAlive || 
            !ev.Player.IsHuman /*||
            ev.Player.IsSubject() */)
            return;

        if (ev.Player.Role == RoleTypeId.Tutorial && !Round.IsRoundStarted) { 
            ev.Player.ShowCoreHint($"<b>{CoinPlugin.Instance!.Config?.DevMessages.RandomItem()}</b>", 3.5f);
            
            return;
        }

        Timing.RunCoroutine(this.TeleportPlayer(ev.Player));
    }

    private IEnumerator<float> TeleportPlayer(Player player) {
        player.AddToTeleporting();

        player.ShowCoreHint("<b>Телепортация.</b>", 1.0f);

        yield return Timing.WaitForSeconds(1);

        player.ShowCoreHint("<b>Телепортация..</b>", 1.0f);

        yield return Timing.WaitForSeconds(1);

        player.ShowCoreHint("<b>Телепортация...</b>", 1.0f);

        yield return Timing.WaitForSeconds(1);

        if (Random.Range(0, 101) < CoinPlugin.Instance!.Config?.TeleportChance) {
            Exiled.API.Features.Player.Get(player).TeleportToRandomRoom(true);
        }
        else {
            player.ShowCoreHint("<b>Не твой день</b>");
        }

        yield return Timing.WaitForSeconds(0.5f);

        player.RemoveFromTeleporting();

        player.RemoveItem(player.CurrentItem!);
    }
}