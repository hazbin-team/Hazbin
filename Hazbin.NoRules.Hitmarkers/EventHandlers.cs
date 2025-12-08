using System.Globalization;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Hazbin.Core.Extensions;
using HintServiceMeow.Core.Enum;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using MEC;
using UnityEngine;
using Player = LabApi.Features.Wrappers.Player;
using Random = UnityEngine.Random;

namespace Hazbin.NoRules.Hitmarkers;

internal class EventHandlers : CustomEventsHandler
{
    internal void OnPlayerHurting(HurtingEventArgs ev)
    {
        if (ev.Attacker == null || ev.Attacker == ev.Player) return;
        
        Timing.CallDelayed(0.1f, () => {
            float amount = ev.Amount;
            
            if (amount > 0 || !ev.IsAllowed || !ev.DamageHandler.IsFriendlyFire || !ev.Player.IsGodModeEnabled)
            {
                Log.Debug($"{ev.Attacker.Role.Team} {ev.Player.Role.Team}");
                
                Player.Get(ev.Attacker.NetworkIdentity).ShowHint($"<b>-{Math.Round(amount, 2).ToString(CultureInfo.CurrentCulture)}</b>", new Vector2(Random.Range(-350, 350), Random.Range(450, 250)), 2.5f, HintVerticalAlign.Middle, HintAlignment.Center, 14, "hit");
            }
        });
    }
        
    public override void OnPlayerDeath(PlayerDeathEventArgs ev)
    {
        if (ev.Attacker == null) return;
       
        ev.Attacker.ShowHint("<b><color=#f24e4e>Убит!</color></b>", new Vector2(Random.Range(-350, 350), Random.Range(500, 400)), 4.0f, HintVerticalAlign.Middle, HintAlignment.Center, 24, "kill");
    }
}