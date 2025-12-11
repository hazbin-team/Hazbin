using Hazbin.Core.Enums;
using Hazbin.Core.Extensions;
using Hazbin.NoRules.PlayerXp.Models;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;

namespace Hazbin.NoRules.PlayerXp;

internal class EventHandlers : CustomEventsHandler
{
    public override void OnPlayerActivatedGenerator(PlayerActivatedGeneratorEventArgs ev) {
        if (ev.Player.DoNotTrack) return;
        
        ev.Player.GiveXp(5);
    }

    public override void OnPlayerInteractedLocker(PlayerInteractedLockerEventArgs ev) {
        if (ev.Player.DoNotTrack) return;

        ev.Player.GiveXp(0.5f);
    }

    public override void OnPlayerInteractedDoor(PlayerInteractedDoorEventArgs ev) {
        if (ev.Player.DoNotTrack || !ev.CanOpen) return;
        
        ev.Player.GiveXp(0.5f);
    }

    public override void OnPlayerPickedUpItem(PlayerPickedUpItemEventArgs ev) {
        if (ev.Player.DoNotTrack) return;

        if (ev.Item.Type is ItemType.MicroHID or ItemType.Jailbird or ItemType.ParticleDisruptor) {
            ev.Player.GiveXp(0.7f);
        }
        else if (ev.Item.Category == ItemCategory.SCPItem) {
            ev.Player.GiveXp(5);
        }
        else if (ev.Item.Category == ItemCategory.SpecialWeapon) {
            ev.Player.GiveXp(7.5f);
        }
        else if (ev.Item.Category == ItemCategory.Firearm) {
            ev.Player.GiveXp(0.5f);
        }
        else if (ev.Item.Category == ItemCategory.Keycard) {
            ev.Player.GiveXp(0.2f);
        }
        else {
            ev.Player.GiveXp(0.1f);
        }
    }

    public override void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev) {
        Level? level = ev.Player.GetLevel();

        if (level != null)
        {
            ev.Player.CustomInfo = ev.Player.DoNotTrack ? $"<color={CustomInfoColor.White.GetHexColor()}>(</color><color={CustomInfoColor.Brown.GetHexColor()}>Неизвестно</color><color={CustomInfoColor.White.GetHexColor()}>)</color>\n{ev.Player.DisplayName.Replace('[', '(').Replace(']', ')')}" 
                : $"<color={CustomInfoColor.White.GetHexColor()}>(</color><color={level.Color.GetHexColor()}>{level.Text}</color><color={CustomInfoColor.White.GetHexColor()}>)</color>\n{ev.Player.DisplayName.Replace('[', '(').Replace(']', ')')}";
        }
        else
        {
            ev.Player.CustomInfo = $"<color={CustomInfoColor.White.GetHexColor()}>(</color><color={CustomInfoColor.Brown.GetHexColor()}>Неизвестно</color><color={CustomInfoColor.White.GetHexColor()}>)</color>\n{ev.Player.DisplayName.Replace('[', '(').Replace(']', ')')}";
        }
        
        ev.Player.InfoArea = (PlayerInfoArea)~(int)PlayerInfoArea.Nickname;
        
        if (ev.Player.DoNotTrack) return;
        
        Timing.RunCoroutine(this.AliveCoroutine(ev.Player), $"aliveXp.{ev.Player.UserId}");

        if (ev.ChangeReason == RoleChangeReason.Escaped)
        {
            if (ev.Player.IsDisarmed)
            {
                ev.Player.DisarmedBy?.GiveXp(100);
            }
            
            ev.Player.GiveXp(150);
        }
    }

    public override void OnPlayerJoined(PlayerJoinedEventArgs ev) {
        if (!XpPlugin.Instance._database!.Contains(ev.Player.UserId) && !ev.Player.DoNotTrack) {
            XpPlugin.Instance._database!.Insert(ev.Player.UserId);
        }

        Timing.CallDelayed(0.45f, () => {
            Level? level = ev.Player.GetLevel();

            if (level != null) {
                if (ev.Player.DisplayName.Contains('[') || ev.Player.DisplayName.Contains(']')) {
                    ev.Player.DisplayName = ev.Player.DisplayName.Replace('[', '(').Replace(']', ')');
                }
                
                ev.Player.CustomInfo = ev.Player.DoNotTrack ? $"<color={CustomInfoColor.White.GetHexColor()}>(</color><color={CustomInfoColor.Brown.GetHexColor()}>Неизвестно</color><color={CustomInfoColor.White.GetHexColor()}>)</color>\n{ev.Player.DisplayName}" 
                    : $"<color={CustomInfoColor.White.GetHexColor()}>(</color><color={level.Color.GetHexColor()}>{level.Text}</color><color={CustomInfoColor.White.GetHexColor()}>)</color>\n{ev.Player.DisplayName}";
            }
            else {
                ev.Player.CustomInfo = $"<color={CustomInfoColor.White.GetHexColor()}>(</color><color={CustomInfoColor.Brown.GetHexColor()}>Неизвестно</color><color={CustomInfoColor.White.GetHexColor()}>)</color>\n{ev.Player.DisplayName}";
            }
        
            ev.Player.InfoArea = (PlayerInfoArea)~(int)PlayerInfoArea.Nickname;
        });
    }

    public override void OnPlayerUsedItem(PlayerUsedItemEventArgs ev) {
        if (ev.UsableItem.Category == ItemCategory.SCPItem) {
            ev.Player.GiveXp(10);
        }
        else {
            ev.Player.GiveXp(0.5f);
        }
    }

    public override void OnPlayerDeath(PlayerDeathEventArgs ev) {
        Timing.KillCoroutines($"aliveXp.{ev.Player.UserId}");

        if (ev.Player.IsSCP) {
            ev.Attacker?.GiveXp(200);
        }
        else if (ev.Attacker is { IsSCP: true }) {
            ev.Attacker?.GiveXp(70);
        }
        else {
            ev.Attacker?.GiveXp(100);
        }
    }

    private IEnumerator<float> AliveCoroutine(Player player)
    {
        while (player.IsAlive)
        {
            yield return Timing.WaitForSeconds(60);
            
            player.GiveXp(1);
        }
    }
}