using LabApi.Features.Wrappers;
using PlayerRoles;

namespace Hazbin.NoRules.BetterEscape.Features;

public static class EscapeScenarioManager {
    private static List<EscapeScenario> _scenarios = new();
    
    public static void UpdateScenarios(List<EscapeScenario> scenarios) => _scenarios = scenarios;
    
    public static RoleTypeId GetScenario(Player player) {
        List<EscapeScenario> scenarios = _scenarios
            .Where(x => x.OldRole == player.Role)
            .Where(x => x.IsCuffed == player.IsDisarmed)
            .ToList();

        if (scenarios.Count <= 0) return RoleTypeId.None;

        return scenarios.RandomItem().NewRole;
    }
}