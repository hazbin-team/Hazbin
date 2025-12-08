using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace Hazbin.NoRules.Old173Spawn;

internal class EventHandlers : CustomEventsHandler {
    public override void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev) {
        if (ev.NewRole.RoleTypeId != RoleTypeId.Scp173 || !ev.SpawnFlags.HasFlag(RoleSpawnFlags.UseSpawnpoint)) return;

        Timing.CallDelayed(0.1f, () => {
            Room room = Room.Get(RoomName.Lcz173).First();
            Vector3 globalPosition = room.Position + room.Transform.TransformDirection(new Vector3(18.7f, 12.0f, 8.0f));

            ev.Player.Position = globalPosition;
        });
    }
}