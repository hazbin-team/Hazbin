using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using MEC;

namespace Hazbin.NoRules.InfinityStuff;

public class EventHandlers : CustomEventsHandler {
    public override void OnPlayerUsingRadio(PlayerUsingRadioEventArgs ev) {
        ev.Drain = 0;
    }

    public override void OnPlayerDroppingAmmo(PlayerDroppingAmmoEventArgs ev) {
        ev.IsAllowed = false;
    }

    public override void OnPlayerReloadingWeapon(PlayerReloadingWeaponEventArgs ev) {
        ev.Player.AddAmmo(ev.FirearmItem.AmmoType, (ushort)(ev.FirearmItem.MaxAmmo - (ev.FirearmItem.ChamberedAmmo + ev.FirearmItem.StoredAmmo)));
    }

    public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev) {
        Timing.CallDelayed(1.0f, () =>
        {
            ev.Player.SetAmmo(ItemType.Ammo762x39, 1);
            ev.Player.SetAmmo(ItemType.Ammo556x45, 1);
            ev.Player.SetAmmo(ItemType.Ammo9x19, 1);
            ev.Player.SetAmmo(ItemType.Ammo44cal, 1);
            ev.Player.SetAmmo(ItemType.Ammo12gauge, 1);
        });
    }

    public override void OnPlayerPickingUpAmmo(PlayerPickingUpAmmoEventArgs ev) {
        if (ev.AmmoPickup.Category != ItemCategory.Ammo) return;
        
        ev.IsAllowed = false;
        ev.AmmoPickup.Destroy();
    }

    public override void OnPlayerCuffing(PlayerCuffingEventArgs ev) {
        ev.Target.ClearAmmo();
    }

    public override void OnPlayerPickingUpItem(PlayerPickingUpItemEventArgs ev) {
        if (ev.Pickup.Category != ItemCategory.Ammo) return;
        
        ev.Pickup.Destroy();
    }

    public override void OnServerPickupCreated(PickupCreatedEventArgs ev) {
        if (ev.Pickup.Category != ItemCategory.Ammo) return;
        
        ev.Pickup.Destroy();
    }
}