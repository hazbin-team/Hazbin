namespace Hazbin.Core.Enums;

public enum DoorPermissionCheck {
    None = 0,
    Bypass = 1,
    Role = 2,
    CurrentItem = 4,
    InventoryExcludingCurrent = 8,
    FullInventory = CurrentItem | InventoryExcludingCurrent,
    All = -1,
    Default = Bypass | Role | CurrentItem,
}