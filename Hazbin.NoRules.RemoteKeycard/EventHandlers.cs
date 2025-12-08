using Interactables.Interobjects.DoorUtils;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;

namespace Hazbin.NoRules.RemoteKeycard;

internal class EventHandlers : CustomEventsHandler
{
    public override void OnPlayerUnlockingGenerator(PlayerUnlockingGeneratorEventArgs ev) {
        if (ev.IsAllowed || ev.Generator.IsUnlocked)
            return;

        ev.IsAllowed = IsPermitted(ev.Player, ev.Generator.RequiredPermissions);
    }

    public override void OnPlayerInteractingLocker(PlayerInteractingLockerEventArgs ev) {
        if (ev.Player.IsBypassEnabled || ev.IsAllowed || !ev.Chamber.CanInteract)
            return;

        ev.IsAllowed = IsPermitted(ev.Player, ev.Chamber.RequiredPermissions);
    }

    public override void OnPlayerInteractingDoor(PlayerInteractingDoorEventArgs ev) {
        if (ev.Player.IsBypassEnabled || ev.IsAllowed || ev.Door.IsLocked || ev.Door.Permissions == DoorPermissionFlags.None)
            return;
        
        if (IsPermitted(ev.Player, ev.Door.Permissions))
        {
            ev.IsAllowed = true;
            ev.CanOpen = true;
            
            return;
        }
        
        ev.IsAllowed = false;
        ev.CanOpen = true;
    }

    private static bool IsPermitted(Player player, DoorPermissionFlags neededPermissions) => GetKeycards(player)
        .Any(x => (neededPermissions & x.Permissions) == neededPermissions);
    
    private static List<KeycardItem> GetKeycards(Player player) =>
        player.Items.OfType<KeycardItem>().ToList();
}