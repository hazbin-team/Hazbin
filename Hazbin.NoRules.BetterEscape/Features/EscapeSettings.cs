using MapGeneration;
using UnityEngine;

namespace Hazbin.NoRules.BetterEscape.Features;

public class EscapeSettings {
    public RoomName BaseRoom { get; set; }
    
    public Vector3 Position { get; set; }
    public Vector3 Scale { get; set; }
    public Vector3 Rotation { get; set; }
}