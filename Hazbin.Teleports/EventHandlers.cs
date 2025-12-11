using Hazbin.Teleports.Extensions;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.Arguments.WarheadEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MapGeneration;
using PlayerRoles;

namespace Hazbin.Teleports;

public sealed class EventHandlers : CustomEventsHandler {
    // -------------- Player --------------
    
    public override void OnPlayerLeft(PlayerLeftEventArgs ev) {
        ev.Player.DenyTeleport();
    }

    public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev) {
        if (ev.NewRole is not RoleTypeId.None and not RoleTypeId.Spectator and not RoleTypeId.Overwatch and not RoleTypeId.CustomRole) {
            ev.Player.AllowTeleport();
        }
        else {
            ev.Player.DenyTeleport();
        }
    }

    // --------------- Map ----------------
    
    public override void OnServerLczDecontaminationStarting(LczDecontaminationStartingEventArgs ev) {
        if (!ev.IsAllowed) return;

        foreach (Room room in TeleportExtensions.Rooms.ToHashSet().Where(room => room.Zone == FacilityZone.LightContainment)) {
            room.DenyTeleport();
        }
    }
    
    // -------------- Server ---------------
    
    public override void OnServerRoundStarted() {
        TeleportExtensions.AllowAllRooms();
    }

    // ------------- Warhead --------------
    
    public override void OnWarheadDetonating(WarheadDetonatingEventArgs ev) {
        TeleportExtensions.DenyAllRooms(RoomName.Outside);
    }
}