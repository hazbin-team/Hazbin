using System.Text.RegularExpressions;
using PlayerRoles;

namespace Hazbin.Core.Extensions;

public static class RoleTypeIdExtensions {
    public static string TranslatedRoleType(this RoleTypeId roleType, bool useColors = false) {
        string ret = roleType switch {
            RoleTypeId.ClassD => "<color=#ff9933>Класс D</color>",
            RoleTypeId.Scientist => "<color=#e7d573>Учёный</color>",
            RoleTypeId.FacilityGuard => "<color=#808080>Охранник</color>",
            RoleTypeId.NtfPrivate => "<color=#3399ff>Рядовой МОГ</color>",
            RoleTypeId.NtfSergeant => "<color=#0066cc>Сержант МОГ</color>",
            RoleTypeId.NtfCaptain => "<color=#0047ab>Капитан МОГ</color>",
            RoleTypeId.NtfSpecialist => "<color=#003366>Специалист МОГ</color>",
            RoleTypeId.Scp049 => "<color=#ff0000>SCP-049</color>",
            RoleTypeId.Scp0492 => "<color=#ff0000>SCP-049-2</color>",
            RoleTypeId.Scp096 => "<color=#ff0000>SCP-096</color>",
            RoleTypeId.Scp106 => "<color=#ff0000>SCP-106</color>",
            RoleTypeId.Scp173 => "<color=#ff0000>SCP-173</color>",
            RoleTypeId.Scp939 => "<color=#ff0000>SCP-939</color>",
            RoleTypeId.Scp079 => "<color=#ff0000>SCP-079</color>",
            RoleTypeId.ChaosConscript => "<color=#228B22>Солдат Хаоса</color>",
            RoleTypeId.ChaosRifleman => "<color=#228B22>Стрелок Хаоса</color>",
            RoleTypeId.ChaosMarauder => "<color=#228B22>Мародёр Хаоса</color>",
            RoleTypeId.ChaosRepressor => "<color=#228B22>Усмиритель Хаоса</color>",
            RoleTypeId.Overwatch => "<color=#00bfff>Надзиратель</color>",
            RoleTypeId.Filmmaker => "<color=#000000>Режиссёр</color>",
            (RoleTypeId)23 => "<color=#ff0000>SCP-3114</color>",
            RoleTypeId.Flamingo => "<color=ff96de>Фламинго</color>",
            RoleTypeId.AlphaFlamingo => "<color=ff1493>Альфа-Фламинго</color>",
            RoleTypeId.ZombieFlamingo => "<color=bfff00>Зомби-Фламинго</color>",
            RoleTypeId.Tutorial => "<color=#ff69b4>Обучение</color>",
            RoleTypeId.Spectator => "<color=#cccccc>Наблюдатель</color>",
            RoleTypeId.None => "<color=#ffffff>Никто</color>",
            RoleTypeId.Destroyed=> "<color=#ffffff>Уничтожен</color>",
            _ => $"{roleType}"
        };
        return useColors ? ret : Regex.Replace(ret, "<[^<>]*>", "");
    }
}