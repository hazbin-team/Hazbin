namespace Hazbin.Core.Enums;

[Flags]
public enum Mask {
    DefaultColliders = 1,
    PlayerCollisionObject = 4,
    NonCollidableHitreg = 64,
    PlayerObjects = 256,
    Pickups = 512,
    DoorButtons = 512,
    PlayerHitbox = 8192,
    Glass = 16384,
    InvisibleWalls = 65536,
    Ragdolls = 131072,
    Scp079CcTV = 262144,
    DoorFrames = 262144,
    ActiveGrenades = 1048576,
    Doors = 134217728,
    ShootThroughWalls = 536870912,
    HitregObstacles = 134496257,
    PlayerCollidable = 671432705,
    Hitreg = 134504449,
    All = 2147483647
}