using Exiled.API.Extensions;
using Exiled.API.Features.Waves;
using Hazbin.Core.Enums;
using Hazbin.Core.Extensions;
using Hazbin.NoRules.Hud.Enums;
using Hazbin.NoRules.PlayerXp;
using Hazbin.NoRules.PlayerXp.Models;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Models.Hints;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp106;
using PlayerStatsSystem;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

namespace Hazbin.NoRules.Hud;

public class EventHandlers : CustomEventsHandler {
    internal static readonly Dictionary<Player, List<Hint>> LocalHints = [];
    
    private static readonly Vector2 RoundTimePos = new(0f, 10f);
    private static readonly Vector2 RoleInfoPos = new(65f, 1065f);
    
    private const string Blue = "#0000FF";
    private const string Green = "#00FF00";

    private static string _specInfo = string.Empty;
    
    internal static readonly Dictionary<AspectRatio, float> AspectRatioToServerName = new()
    {
        {AspectRatio.SixteenToNine, -338.0f},
        {AspectRatio.FourToThree, -100.0f},
        {AspectRatio.SixteenToTen, -240.0f}
    };

    internal static readonly Dictionary<AspectRatio, float> AspectRatioToRoleInfo = new()
    {
        {AspectRatio.SixteenToNine, 65.0f},
        {AspectRatio.FourToThree, 295.0f},
        {AspectRatio.SixteenToTen, 150.0f}
    };

    public static readonly Dictionary<AspectRatio, float> AspectRatioToScpPos = new()
    {
        {AspectRatio.SixteenToNine, 856.0f},
        {AspectRatio.FourToThree, 839.0f},
        {AspectRatio.SixteenToTen, 959.0f}
    };

    public override void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev) {
        if (RoleExtensions.GetTeam(ev.NewRole.RoleTypeId) == Team.SCPs)
        {
            SetScpStatus(ev.Player, 0); // ставлю статус на чат дцп когда человека респавнит чтобы да
            return; // усё, досвидания
        }
        
        SetScpStatus(ev.Player, 2); // если новая роль не дцп тогда сбрасываю статус на пустую строку
    }

    public override void OnPlayerJoined(PlayerJoinedEventArgs ev) {
        TimeSpan time = Round.Duration;
        
        string formattedTime = time.Hours > 0 ? time.ToString(@"hh\:mm\:ss") : time.ToString(@"mm\:ss");
        string tpsText = $"Раунд идёт: {formattedTime} | TPS: {Server.Tps}/{Server.MaxTps}";
        string project = "<color=#F92525>H</color><color=#F93B23>a</color><color=#F95121>z</color><color=#F9671F>b</color><color=#F97D1D>i</color><color=#F9931B>n</color>";
        string serverName = HudPlugin.Instance!.Config.ServerName;
        string header = $"<b>{project} <size=20>{serverName}</size></b>";

        string groupInfo = "";
        string group = ev.Player.ReferenceHub.serverRoles.GetColoredRoleString();
        if (!string.IsNullOrEmpty(group))
        {
            groupInfo = $"\n<color=#C93E3E>Права</color>: {group}";
        }

        string role = ev.Player.IsAlive
            ? ev.Player.Role.TranslatedRoleType(true)
            : ev.Player.Role.TranslatedRoleType();
        
        Level? level = ev.Player.GetLevel();
        
        string levelText = level != null 
            ? $"<color={level.Color.GetHexColor()}>{level.Text}</color>" 
            : "<color=#888888>Неизвестно</color>";
        
        string info = $"<color=#C93E3E>Вы</color>: {ev.Player.Nickname}\n<color=#C93E3E>Роль</color>: {role}\n<color=#C93E3E>Уровень</color>: {levelText} {groupInfo}";
        
        Hint roundTimeHint = ev.Player.ShowHint(tpsText, RoundTimePos, 99999.9f, fontSize: 16, tag: "roundTime");
        
        Hint serverNameHint = ev.Player.ShowHint(header, new Vector2(AspectRatioToServerName[GetAspectRatio(ev.Player)], 1010.0f), 99999.9f, HintVerticalAlign.Bottom, HintAlignment.Left, 48, "serverHud");
        
        Hint playerInfoHint = ev.Player.ShowHint(info, new Vector2(AspectRatioToRoleInfo[GetAspectRatio(ev.Player)], RoleInfoPos.y), 99999.9f, HintVerticalAlign.Bottom,
            HintAlignment.Left, 20, "serverHud");
        
        Hint scpTextHint = ev.Player.ShowHint(string.Empty,
            new Vector2(AspectRatioToScpPos[GetAspectRatio(ev.Player)], 975), 99999f, HintVerticalAlign.Middle,
            HintAlignment.Left, 24, "proximityChat");

        Hint scpInfoHint = ev.Player.ShowHint(string.Empty,
            new Vector2(AspectRatioToScpPos[GetAspectRatio(ev.Player)], 400), 99999f, HintVerticalAlign.Middle,
            HintAlignment.Left, 26, "scpInfo");
        
        Hint respawnTimerHint = ev.Player.ShowHint(string.Empty, new Vector2(0, 925), 99999.9f,
            HintVerticalAlign.Middle, tag: "respawnTimer");
        
        LocalHints[ev.Player] = 
        [
            roundTimeHint,
            serverNameHint,
            playerInfoHint,
            scpTextHint,
            respawnTimerHint,
            scpInfoHint
        ];
        
        SetScpStatus(ev.Player, 2);
    }

    public override void OnPlayerLeft(PlayerLeftEventArgs ev) {
        LocalHints.Remove(ev.Player);
    }

    public static void SetScpStatus(Player player, int status)
    {
        if (!LocalHints.ContainsKey(player))
            return;
        
        switch (status)
        {
            case 0: // ну типо 0 это фолз
                LocalHints[player][3].Text = "<b>Вы говорите в <color=red>чат SCP</color></b>";
                return;
            case 1: // а 1 это тру
                LocalHints[player][3].Text = "<b>Вы говорите в <color=green>общий чат</color></b>";
                return;
            default: // а это типо чтобы ресетать хинт
                LocalHints[player][3].Text = string.Empty;
                LocalHints[player][5].Text = string.Empty;
                return;
        }
    }

    internal static IEnumerator<float> SpecInfoSelector()
    {
        while (true)
        {
            _specInfo = HudPlugin.Instance!.Config!.SpectatorInfo.RandomItem();
            yield return Timing.WaitForSeconds(HudPlugin.Instance.Config.UpdateInfoInterval);
        }
    }

    internal static IEnumerator<float> UpdateRespawnTimer()
    {
        while (Round.IsRoundInProgress)
        {
            try
            {
                foreach (Hint hint in LocalHints.Values.Where(x => x.Count >= 5).Select(x => x[4]))
                {
                    Player player = LocalHints.First(x => x.Value.Contains(hint)).Key;

                    if (player.IsAlive || !Round.IsRoundStarted)
                    {
                        if (hint.Text != string.Empty)
                            hint.Text = string.Empty;
                        
                        continue;
                    }
                    
                    TimeSpan ntfTime = WaveTimer.GetWaveTimers().First(x => x.Name.ToLower().Contains("ntf") && !x.IsMiniWave).TimeLeft;
                    TimeSpan ntfMiniTime = WaveTimer.GetWaveTimers().First(x => x.Name.ToLower().Contains("ntf") && x.IsMiniWave).TimeLeft;
                    TimeSpan chaosTime = WaveTimer.GetWaveTimers().First(x => x.Name.ToLower().Contains("chaos") && !x.IsMiniWave).TimeLeft;
                    TimeSpan chaosMiniTime = WaveTimer.GetWaveTimers().First(x => x.Name.ToLower().Contains("chaos") && x.IsMiniWave).TimeLeft;

                    hint.Text =
                        $"<b><size=20><color=#FFA500>Вы заспавнитесь через:</color>\n<color={Blue}>МОГ</color>: {ntfTime.Minutes:D2} мин. {ntfTime.Seconds:D2}с.\n<color={Blue}>Подкрепление МОГ</color>: {ntfMiniTime.Minutes:D2} мин. {ntfMiniTime.Seconds:D2}с.\n<color={Green}>ПХ</color>: {chaosTime.Minutes:D2} мин. {chaosTime.Seconds:D2}с.\n<color={Green}>Подкрепление ПХ</color>: {chaosMiniTime.Minutes:D2} мин. {chaosMiniTime.Seconds:D2}с.\n\n\n\n\n\n{_specInfo}</size></b>";
                }
            }
            catch (Exception e)
            {
                Logger.Error($"[HUD] Coroutine Error: {e}");
            }
            
            yield return Timing.WaitForSeconds(0.95f);
        }
    }

    internal static IEnumerator<float> UpdateScpInfo()
    {
        while (true)
        {
            try
            {
                foreach (Hint hint in LocalHints.Values.Where(x => x.Count >= 4).Select(x => x[3]))
                {
                    Player player = LocalHints.First(x => x.Value.Contains(hint)).Key;

                    if (!Mathf.Approximately(hint.XCoordinate, AspectRatioToScpPos[GetAspectRatio(player)]))
                        hint.XCoordinate = AspectRatioToScpPos[GetAspectRatio(player)];
                }
                
                foreach (Hint hint in LocalHints.Values.Where(x => x.Count >= 6).Select(x => x[5]))
                {
                    Player player = LocalHints.First(x => x.Value.Contains(hint)).Key;
                    
                    if (!player.IsSCP)
                        continue;
                    
                    string text = Player.List.Where(x => x.IsSCP).Aggregate(string.Empty,
                        (current, scp) =>
                            current +
                            $"{scp.Role.TranslatedRoleType(true)} <color=#FFFFFF>[<color=yellow>{Math.Round(scp.Health, 1)}</color>]</color>\n");

                    text = text.TrimEnd('\n');

                    if (hint.Text != text)
                        hint.Text = text;
                        
                    if (!Mathf.Approximately(hint.XCoordinate, AspectRatioToScpPos[GetAspectRatio(player)]))
                        hint.XCoordinate = AspectRatioToScpPos[GetAspectRatio(player)];
                }
            }
            catch (Exception e)
            {
                Logger.Error($"[HUD] Coroutine Error: {e}");
            }
            
            yield return Timing.WaitForSeconds(0.45f);
        }
    }

    internal static IEnumerator<float> UpdateServerNamePos()
    {
        while (true)
        {
            try
            {
                foreach (Hint hint in LocalHints.Values.Where(x => x.Count >= 2).Select(x => x[1]))
                {
                    Player player = LocalHints.First(x => x.Value.Contains(hint)).Key;
                    Vector2 cords = new Vector2(LocalHints[player][1].XCoordinate, LocalHints[player][1].YCoordinate);
                    if (GetPosition(player) == cords)
                        continue;

                    cords = GetPosition(player);
                    
                    hint.XCoordinate = cords.x;
                    
                    if (!Mathf.Approximately(hint.XCoordinate, AspectRatioToServerName[GetAspectRatio(player)]))
                        hint.XCoordinate = AspectRatioToServerName[GetAspectRatio(player)];
                    
                    hint.YCoordinate = cords.y;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"[HUD] Coroutine error: {e}");
            }

            yield return Timing.WaitForSeconds(0.45f);
        }
    }

    internal static IEnumerator<float> ShowPlayerInfo()
    {
        while (true)
        {
            try
            {
                foreach (Hint hint in LocalHints.Values.Where(x => x.Count >= 3).Select(x => x[2]))
                {
                    Player player = LocalHints.First(x => x.Value.Contains(hint)).Key;

                    string groupInfo = "";
                    string group = player.ReferenceHub.serverRoles.GetColoredRoleString();
                    if (!string.IsNullOrEmpty(group)) {
                        groupInfo = $"\n<color=#C93E3E>Права</color>: {group}";
                    }

                    string role = player.IsAlive
                        ? player.Role.TranslatedRoleType(true)
                        : player.Role.TranslatedRoleType();
        
                    Level? level = player.GetLevel();
                    string levelText = level != null 
                        ? $"<color={level.Color.GetHexColor()}>{level.Text}</color>" 
                        : $"<color={CustomInfoColor.Brown.GetHexColor()}>Неизвестно</color>";
                    
                    string info = $"<color=#C93E3E>Вы</color>: {player.Nickname}\n<color=#C93E3E>Роль</color>: {role}\n<color=#C93E3E>Уровень</color>: {levelText} {groupInfo}";

                    if (!Mathf.Approximately(hint.XCoordinate, AspectRatioToRoleInfo[GetAspectRatio(player)]))
                        hint.XCoordinate = AspectRatioToRoleInfo[GetAspectRatio(player)];
                    
                    if (LocalHints[player][2].Text == info)
                        continue;
                    
                    hint.Text = info;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"[HUD] Coroutine error: {e}");
            }

            yield return Timing.WaitForSeconds(0.45f);
        }
    }

    internal static IEnumerator<float> ShowRoundTime()
    {
        while (true)
        {
            try
            {
                TimeSpan time = Round.Duration;
                string formattedTime = time.Hours > 0 ? time.ToString(@"hh\:mm\:ss") : time.ToString(@"mm\:ss");
                string tpsText = $"Раунд идёт: {formattedTime} | TPS: {Server.Tps}/{Server.MaxTps}";
                
                foreach (Hint hint in LocalHints.Values.Where(x => x.Count > 0).Select(x => x[0]))
                {
                    hint.Text = tpsText;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"[HUD] Coroutine error: {e}");
            }

            yield return Timing.WaitForSeconds(0.95f);
        }
    }

    private static Vector2 GetPosition(Player player)
    {
        float y = 1010f;

        Player? spectating = Player.List.FirstOrDefault(p => p.CurrentSpectators.Contains(player));
        bool isSpectating = !player.IsAlive && spectating != null;

        float staminaPenalty = (isSpectating ? spectating : player)!.GetStatModule<StaminaStat>().CurValue < player.GetStatModule<StaminaStat>().MaxValue ? 23f : 0f;
        float humePenalty = (isSpectating ? spectating : player)!.MaxHumeShield > 0 ? 30f : 0f;
        float ahpPenalty = (isSpectating ? spectating : player)!.ArtificialHealth > 0 ? 30f : 0f;
        float scp106Penalty = (isSpectating ? spectating : player)!.Role is Scp106Role ? 23f : 0f;

        y -= staminaPenalty + humePenalty + ahpPenalty + scp106Penalty;
        if (!player.IsAlive && spectating == null)
            y -= 50f;

        return new Vector2(AspectRatioToServerName[GetAspectRatio(player)], y);
    }
    
    public static AspectRatio GetAspectRatio(Player player)
    {
        float aspectRatio = player.ReferenceHub.aspectRatioSync.AspectRatio;
        if (Mathf.Approximately(aspectRatio, 16.0f / 9.0f))
            return AspectRatio.SixteenToNine;
        
        if (Mathf.Approximately(aspectRatio, 4.0f / 3.0f))
            return AspectRatio.FourToThree;
        
        if (Mathf.Approximately(aspectRatio, 16.0f / 10.0f))
            return AspectRatio.SixteenToTen;
        
        return AspectRatio.SixteenToNine;
    }
}