using PlayerRoles;

namespace Hazbin.NoRules.BetterEscape.Features;

public sealed class EscapeScenario {
    public RoleTypeId OldRole { get; set; }
    public RoleTypeId NewRole { get; set; }
    public bool IsCuffed { get; set; }
}