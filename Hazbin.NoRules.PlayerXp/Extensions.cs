using Hazbin.Core.Enums;
using Hazbin.Core.Extensions;
using Hazbin.NoRules.PlayerXp.Models;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Models.Hints;
using LabApi.Features.Wrappers;
using UnityEngine;
using Hint = HintServiceMeow.Core.Models.Hints.Hint;

namespace Hazbin.NoRules.PlayerXp;

public static class Extensions
{
    public static void GiveXp(this Player player, float exp)
    {
        exp /= 3;
        
        if (player.Nickname.Contains("#VORTEX"))
        {
            exp *= 2;
        }

        Models.PlayerXp? playerXp = XpPlugin.Instance._database!.Find(player.UserId);
        if (playerXp is { Experience: >= 0 })
        {
            XpPlugin.Instance._database!.GiveXp(player.UserId, exp);
        }
        
        List<AbstractHint>? hints = player.GetHintsByTag("xp");
    
        if (hints != null)
        {
            foreach (AbstractHint absHint in hints)
            {
                if (absHint is Hint hint)
                {
                    hint.YCoordinate = Math.Max(0, hint.YCoordinate - 27);
                }
            }
        }
        
        player.ShowHint($"<b>Вы получили <color=#ffe91f>{Math.Round(exp, 2)}</color> опыта!</b>", new Vector2(AspectRatioToScpPos[GetAspectRatio(player)], 925), 
            2.4f, HintVerticalAlign.Middle, HintAlignment.Left, 24, "xp");
        
        Level? level = player.GetLevel();

        if (level != null) {
            player.CustomInfo = player.DoNotTrack ? $"<color={CustomInfoColor.White.GetHexColor()}>(</color><color={CustomInfoColor.Brown.GetHexColor()}>Неизвестно</color><color={CustomInfoColor.White.GetHexColor()}>)</color>\n{player.DisplayName}" 
                : $"<color={CustomInfoColor.White.GetHexColor()}>(</color><color={level.Color.GetHexColor()}>{level.Text}</color><color={CustomInfoColor.White.GetHexColor()}>)</color>\n{player.DisplayName}";
        }
        else {
            player.CustomInfo = $"<color={CustomInfoColor.White.GetHexColor()}>(</color><color={CustomInfoColor.Brown.GetHexColor()}>Неизвестно</color><color={CustomInfoColor.White.GetHexColor()}>)</color>\n{player.DisplayName}";
        }
        
        player.InfoArea = (PlayerInfoArea)~(int)PlayerInfoArea.Nickname;
    }

    public static Level? GetLevel(this Player player) {
        Models.PlayerXp? xp = XpPlugin.Instance._database!.Find(player.UserId);
        
        if (xp != null) {
            return XpPlugin.Instance._database!.GetLevel(player.UserId)!;
        }
        
        return null;
    }
    
    private static readonly Dictionary<AspectRatio, float> AspectRatioToScpPos = new()
    {
        {AspectRatio.SixteenToNine, 860.0f},
        {AspectRatio.FourToThree, 843.0f},
        {AspectRatio.SixteenToTen, 973.0f}
    };
    
    private static AspectRatio GetAspectRatio(Player player)
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
    
    private enum AspectRatio {
        SixteenToNine,
        SixteenToTen,
        FourToThree,
    }
}