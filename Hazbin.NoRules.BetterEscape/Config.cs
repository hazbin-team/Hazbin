using Hazbin.NoRules.BetterEscape.Features;
using MapGeneration;
using PlayerRoles;
using UnityEngine;

namespace Hazbin.NoRules.BetterEscape;

public class Config {
    public List<EscapeSettings> Escapes { get; set; } = new() {
        new EscapeSettings {
            BaseRoom = RoomName.Outside,
            Position = new Vector3(123.8f, 988.884f, 17f),
            Rotation = Vector3.zero,
            Scale = new Vector3(4, 4, 0.1f)
        },
        new EscapeSettings {
            BaseRoom = RoomName.Outside,
            Position = new Vector3(-41.3f, 991.881f, -36.1f),
            Rotation = Vector3.zero,
            Scale = new Vector3(1, 2.5f, 1.5f)
        }
    };

    public List<EscapeScenario> Scenarios { get; set; } = new() {
        new EscapeScenario 
        { 
            OldRole = RoleTypeId.ClassD,
            NewRole = RoleTypeId.ChaosRifleman,
            IsCuffed = false
        },
        new EscapeScenario
        { 
            OldRole = RoleTypeId.ClassD,
            NewRole = RoleTypeId.NtfPrivate,
            IsCuffed = true
        },
        new EscapeScenario {
            OldRole = RoleTypeId.Scientist,
            NewRole = RoleTypeId.NtfSpecialist,
            IsCuffed = false
        },
        new EscapeScenario {
            OldRole = RoleTypeId.Scientist,
            NewRole = RoleTypeId.ChaosConscript,
            IsCuffed = true
        },
        new EscapeScenario {
            OldRole = RoleTypeId.FacilityGuard,
            NewRole = RoleTypeId.NtfSergeant,
            IsCuffed = false
        },
        new EscapeScenario {
            OldRole = RoleTypeId.FacilityGuard,
            NewRole = RoleTypeId.ChaosConscript,
            IsCuffed = true
        },
        new EscapeScenario {
            OldRole = RoleTypeId.NtfPrivate,
            NewRole = RoleTypeId.ChaosConscript,
            IsCuffed = true
        },
        new EscapeScenario {
            OldRole = RoleTypeId.NtfSergeant,
            NewRole = RoleTypeId.ChaosRifleman,
            IsCuffed = true
        },
        new EscapeScenario {
            OldRole = RoleTypeId.NtfSpecialist,
            NewRole = RoleTypeId.ChaosMarauder,
            IsCuffed = true
        },
        new EscapeScenario {
            OldRole = RoleTypeId.NtfCaptain,
            NewRole = RoleTypeId.ChaosRepressor,
            IsCuffed = true
        },
        new EscapeScenario {
            OldRole = RoleTypeId.ChaosConscript,
            NewRole = RoleTypeId.NtfPrivate,
            IsCuffed = true
        },
        new EscapeScenario {
            OldRole = RoleTypeId.ChaosRifleman,
            NewRole = RoleTypeId.NtfSergeant,
            IsCuffed = true
        },
        new EscapeScenario {
            OldRole = RoleTypeId.ChaosMarauder,
            NewRole = RoleTypeId.NtfSpecialist,
            IsCuffed = true
        },
        new EscapeScenario {
            OldRole = RoleTypeId.ChaosRepressor,
            NewRole = RoleTypeId.NtfCaptain,
            IsCuffed = true
        }
    };
}