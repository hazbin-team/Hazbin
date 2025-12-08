using PlayerRoles;

namespace Hazbin.NoRules.ProximityChat;

public class Config {
    public HashSet<RoleTypeId> AllowedRoles { get; set; } = [
        RoleTypeId.Scp049,
        RoleTypeId.Scp096,
        RoleTypeId.Scp106,
        RoleTypeId.Scp173,
        RoleTypeId.Scp0492,
        RoleTypeId.Scp939,
        RoleTypeId.Scp3114
    ];
}