using CustomPlayerEffects;
using Hazbin.Core.Extensions;
using Hazbin.Teleports.Extensions;
using LabApi.Features.Wrappers;
using MEC;
using UnityEngine;

namespace Hazbin.NoRules.Scp120;

public class WateryChecker : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        Player? player = Player.Get(other.gameObject);
        if (player == null) return;

        Timing.CallDelayed(1.75f, () => Exiled.API.Features.Player.Get(player).TeleportToRandomRoom());

        player.EnableEffect<Flashed>(1, 2f);
        player.ShowCoreHint("<b>Вы таинственным образом переместились в случайную комнату!</b>");
    }
}