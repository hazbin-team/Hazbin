using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using ProjectMER.Features;
using ProjectMER.Features.Objects;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

namespace Hazbin.NoRules.Scp120;

internal class EventHandlers : CustomEventsHandler {
    public override void OnServerRoundStarted() {
        Timing.CallDelayed(0.5f, () => {
            Room room = Room.Get(RoomName.LczGlassroom).First();
        
            SchematicObject schematic = ObjectSpawner.SpawnSchematic("gr18", room.Position, room.Rotation, Vector3.one);

            GameObject? obj = schematic.AttachedBlocks.FirstOrDefault(x => x.name.Equals("watery", StringComparison.OrdinalIgnoreCase));
            if (obj != null) {
                obj.AddComponent<CapsuleCollider>().isTrigger = true;
                obj.AddComponent<WateryChecker>();
            }
        });
    }
}