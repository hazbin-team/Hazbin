using Interactables.Interobjects;

namespace Hazbin.Core.Features;

public static class PrefabManager {
    private const string LczDoorName = "LCZ BreakableDoor";
    private const string HczDoorName = "HCZ BreakableDoor";
    private const string HczBulkDoorName = "HCZ BulkDoor";
    private const string EzDoorName = "EZ BreakableDoor";

    private static BasicDoor? _lczDoor;
    private static BasicDoor? _hczDoor;
    private static BasicDoor? _hczBulkDoor;
    private static BasicDoor? _ezDoor;

    public static ReferenceHub PlayerPrefab => PrefabStore<ReferenceHub>.Prefab;

    public static BasicDoor LczDoorPrefab => _lczDoor ??= GetDoor(LczDoorName);

    public static BasicDoor HczDoorPrefab => _hczDoor ??= GetDoor(HczDoorName);

    public static BasicDoor HczBulkDoorPrefab => _hczBulkDoor ??= GetDoor(HczBulkDoorName);

    public static BasicDoor EzDoorPrefab => _ezDoor ??= GetDoor(EzDoorName);

    private static BasicDoor GetDoor(string name)
        => PrefabStore<BasicDoor>.AllComponentPrefabs.FirstOrDefault(d => d.name == name)!;
}