using System.Globalization;
using Exiled.API.Features.DamageHandlers;
using Hazbin.Core.Extensions;
using HintServiceMeow.Core.Enum;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hazbin.NoRules.Hitmarkers;

internal class EventHandlers : CustomEventsHandler {
    public override void OnPlayerHurt(PlayerHurtEventArgs ev) {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (ev.Attacker == null || ev.Player == null) return;
        
        CustomDamageHandler dh = new(ev.Player, ev.DamageHandler);
        
        if (dh.Damage > 0 && (Server.FriendlyFire || ev.Attacker.Faction != ev.Player.Faction) && !ev.Player.IsGodModeEnabled) {
            ev.Attacker.ShowHint($"<b>-{Math.Round(dh.Damage, 2).ToString(CultureInfo.CurrentCulture)}</b>", new Vector2(Random.Range(-350, 350), Random.Range(450, 250)), 2.5f, HintVerticalAlign.Middle, HintAlignment.Center, 14, "hit");
        }
    }

    public override void OnPlayerDeath(PlayerDeathEventArgs ev) {
        if (ev.Attacker == null) return;
       
        ev.Attacker.ShowHint("<b><color=#f24e4e>Убит!</color></b>", new Vector2(Random.Range(-350, 350), Random.Range(500, 400)), 4.0f, HintVerticalAlign.Middle, HintAlignment.Center, 24, "kill");
    }
}