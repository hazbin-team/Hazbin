using Exiled.API.Enums;
using Hazbin.Teleports.Extensions;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.Arguments.WarheadEvents;
using LabApi.Events.CustomHandlers;
using PlayerRoles;
using Room = Exiled.API.Features.Room;

namespace Hazbin.Teleports;

public sealed class EventHandlers : CustomEventsHandler {
    // -------------- Player --------------
    
    public override void OnPlayerLeft(PlayerLeftEventArgs ev) {
        Exiled.API.Features.Player.Get(ev.Player).DenyTeleport();
    }

    public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev) {
        if (ev.NewRole is not RoleTypeId.None and not RoleTypeId.Spectator and not RoleTypeId.Overwatch and not RoleTypeId.CustomRole) {
            Exiled.API.Features.Player.Get(ev.Player).AllowTeleport();
        }
        else {
            Exiled.API.Features.Player.Get(ev.Player).DenyTeleport();
        }
    }

    public override void OnPlayerFlippedCoin(PlayerFlippedCoinEventArgs ev) {
        TeleportExtensions.DenyAllRooms(RoomType.Pocket);
        Exiled.API.Features.Player.Get(ev.Player).TeleportToRandomRoom();
        ev.Player.SendHint($"{ev.IsTails} | {ev.CoinItem.LastFlipTime} | {ev.CoinItem.LastFlipResult}");
        ev.Player.RemoveItem(ev.CoinItem);
        TeleportExtensions.AllowAllRooms();
    }

    // --------------- Map ----------------
    
    public override void OnServerLczDecontaminationStarting(LczDecontaminationStartingEventArgs ev) {
        if (!ev.IsAllowed) return;

        foreach (Room room in TeleportExtensions.Rooms.ToHashSet().Where(room => room.Zone == ZoneType.LightContainment)) {
            room.DenyTeleport();
        }
    }
    
    // -------------- Server ---------------
    
    public override void OnServerRoundStarted() {
        TeleportExtensions.AllowAllRooms();
    }

    // ------------- Warhead --------------
    
    public override void OnWarheadDetonating(WarheadDetonatingEventArgs ev) {
        TeleportExtensions.DenyAllRooms(RoomType.Surface);
    }
}