using CustomPlayerEffects;
using LabApi.Events.Arguments.Scp914Events;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using PlayerRoles;
using Scp914;
using Random = UnityEngine.Random;

namespace Hazbin.NoRules.Scp914Rework;

internal class EventHandlers : CustomEventsHandler {
    public override void OnScp914ProcessingPlayer(Scp914ProcessingPlayerEventArgs ev) {
        if (Player.List.Where(x => x.IsSCP && x.Role != RoleTypeId.Scp0492).Contains(ev.Player)) return;
        
        switch (ev.KnobSetting) {
            case Scp914KnobSetting.Rough: {
                ev.Player.EnableEffect<CardiacArrest>(5, 10.0f);

                if (Random.Range(0, 100) < 15 && !ev.Player.IsSCP /*&& !ev.Player.IsSubject()*/) {
                    ev.Player.SetRole(RoleTypeId.Scp0492);
                }
                break;
            }
            case Scp914KnobSetting.Coarse: {
                ev.Player.EnableEffect<Poisoned>(10, 2.5f);
                ev.Player.EnableEffect<AmnesiaItems>(200, 10.0f);
                ev.Player.EnableEffect<AmnesiaVision>(200, 10.0f);
                break;
            }
            case Scp914KnobSetting.OneToOne: {
                ev.Player.EnableEffect<Blurred>(10, 5.0f);
                break;
            }
            case Scp914KnobSetting.Fine: {
                ev.Player.EnableEffect<Invigorated>(100, 10.0f);
                ev.Player.EnableEffect<MovementBoost>(5, 10.0f);
                break;
            }
            case Scp914KnobSetting.VeryFine: {
                if (Random.Range(0, 100) < 15 && ev.Player.Role == RoleTypeId.Scp0492) {
                    ev.Player.SetRole(RoleTypeId.ClassD);
                    break;
                }
                
                ev.Player.EnableEffect<RainbowTaste>(1, 10.0f);
                ev.Player.EnableEffect<Invigorated>(100, 10.0f);
                ev.Player.EnableEffect<MovementBoost>(5, 10.0f);
                ev.Player.EnableEffect<BodyshotReduction>(10, 15.0f);
                break;
            }
        }
    }
}