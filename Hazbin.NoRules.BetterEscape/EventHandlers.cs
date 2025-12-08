using Hazbin.NoRules.BetterEscape.Components;
using Hazbin.NoRules.BetterEscape.Features;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

namespace Hazbin.NoRules.BetterEscape;

public class EventHandlers(Config config) : CustomEventsHandler {
    public override void OnPlayerEscaping(PlayerEscapingEventArgs ev) => ev.IsAllowed = false;

    public override void OnServerRoundStarted() {
        Logger.Debug("Round started!");
        Timing.CallDelayed(0.5f, () => {
            Logger.Debug("Spawning escapes");
            foreach (EscapeSettings escape in config.Escapes) {
                Logger.Debug("Spawning escape on " + escape.Position);
                
                Room room = Room.Get(escape.BaseRoom).First();
                
                Logger.Debug("Room is " + room);
                
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.isStatic = true;
                obj.transform.parent = room.Transform;
                obj.transform.localPosition = escape.Position;
                obj.transform.localRotation = Quaternion.Euler(escape.Rotation);
                obj.transform.localScale = escape.Scale;
                
                obj.GetComponent<BoxCollider>().isTrigger = true;
                obj.AddComponent<EscapeComponent>();
                
                Logger.Debug("Spawned escape on: " + escape.Position);
            }
        });
    }
}