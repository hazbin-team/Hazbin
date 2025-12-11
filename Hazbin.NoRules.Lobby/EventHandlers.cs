using System.Reflection;
using GameCore;
using Hazbin.Core.Enums;
using Hazbin.Core.Extensions;
using HintServiceMeow.Core.Enum;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using Mirror;
using PlayerRoles;
using PlayerRoles.RoleAssign;
using ProjectMER.Features;
using UnityEngine;

namespace Hazbin.NoRules.Lobby;

internal class EventHandlers(string soundsPath) : CustomEventsHandler {
    private AudioPlayer? lobbyPlayer;

    private readonly Dictionary<Player, LobbyInteractionBlocker> _blockers = [];

    public override void OnServerWaitingForPlayers() {
        string path = Path.Combine(soundsPath, "lobby");
        string[] files = Directory.GetFiles(path, "*.ogg");

        if (files.Length == 0)
            return;

        List<string> clips = [];
        foreach (string file in files) {
            string name = Path.GetFileNameWithoutExtension(file);
            AudioClipStorage.LoadClip(file, name);
            clips.Add(name);
        }

        Timing.CallDelayed(1f, () => {
            ObjectSpawner.SpawnSchematic("hazbin", new (0, -50, 0));

            AudioPlayer? player = AudioPlayer.CreateOrGet("Lobby",
                onIntialCreation: p => {
                    Speaker? speaker = p.AddSpeaker("Main", isSpatial: false, maxDistance: 1000f);
                    speaker.Position = new Vector3(0, -50, 0) + Vector3.up;
                });

            clips = clips.OrderBy(_ => UnityEngine.Random.value).ToList();

            foreach (string? c in clips)
                player.AddClip(c, 5f);
        });
    }

    public override void OnServerRoundStarted() {
        foreach (Player player in Player.List.Where(x => !x.IsHost && x.Role != RoleTypeId.Overwatch)) {
            player.ReferenceHub.interCoordinator.RemoveBlocker(this._blockers[player]);
            player.DisableEffect(EffectType.MovementBoost);
            player.DisableEffect(EffectType.SilentWalk);
            player.IsGodModeEnabled = false;
            player.ClearItems();
            player.SetRole(RoleTypeId.Spectator);
            player.ClearHints("lobby");
        }

        this.lobbyPlayer?.Destroy();
        
        Timing.KillCoroutines("lobby");
        
        Type type = typeof(RoleAssigner);
        MethodInfo? method = type.GetMethod("OnRoundStarted", BindingFlags.NonPublic | BindingFlags.Static);

        if (method != null) {
            method.Invoke(null, null);
        }
    }

    public override void OnPlayerJoined(PlayerJoinedEventArgs ev) {
        if (Round.IsRoundInProgress || ev.Player.Role == RoleTypeId.Overwatch) return;

        if (GameObject.Find("StartRound").TryGetComponent(out NetworkIdentity identity)) {
            ev.Player.ReferenceHub.connectionToClient.Send(new ObjectDestroyMessage {netId = identity.netId});
        }
        
        ev.Player.SetRole(RoleTypeId.Tutorial);
        Exiled.API.Features.Player.Get(ev.Player).Teleport(new Vector3(0.0f, -48.5f, 0.0f));
        
        ev.Player.EnableEffect(EffectType.MovementBoost, 100);
        ev.Player.EnableEffect(EffectType.SilentWalk, 255);
        
        ev.Player.IsGodModeEnabled = true;
        ev.Player.CurrentItem = ev.Player.AddItem(ItemType.Coin);
        
        this._blockers[ev.Player] = new LobbyInteractionBlocker();
        ev.Player.ReferenceHub.interCoordinator.AddBlocker(this._blockers[ev.Player]);
        
        Timing.RunCoroutine(this.ShowLobbyText(ev.Player), "lobby");
    }

    public override void OnPlayerLeft(PlayerLeftEventArgs ev) {
        if (this._blockers.ContainsKey(ev.Player))
            this._blockers.Remove(ev.Player);
    }

    private IEnumerator<float> ShowLobbyText(Player player) {
        for (;;) { 
            player.ShowHint($"<b><color=#FF8C00>Ждем демонят, {(Round.IsLobbyLocked ? "Раунд приостановлен." : Server.PlayerCount > 1 ? RoundStart.singleton.NetworkTimer < 1 ? "ДА НАЧНЕТСЯ ЖАТВААА!" : $"{RoundStart.singleton.NetworkTimer} секунд(ы) осталось." : "Вы пока одни, \nно скоро к вам зайдут другие демонята!")}</color>\n" +
                          $"<color=#800080><i>{Server.PlayerCount} ✞демон(-ов) присоединилось✞.</i></color></b>", new Vector2(0, 250), 1.01f, HintVerticalAlign.Middle, HintAlignment.Center, 40, "lobby");
            
            yield return Timing.WaitForSeconds(1.0f);
        }
    }
}