using CommandSystem;
using Hazbin.Core.Features;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using PlayerRoles;

namespace Hazbin.NoRules.DotResKill.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
internal sealed class Respawn : ICommand
{
    public string Command => "Respawn";

    public string[] Aliases { get; } = ["Res"];

    public string Description => "Возродитесь за класс D или научного сотрудника в первые минуты игры";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        Player.TryGet(sender, out Player player);
        if (player == null) {
            response = "Вы не являетесь игроком. Скорее всего вы пишите команду в консоли сервера.";
            return false;
        }
        
        if (!DotPlugin.ResIsAllowed) {
            response = ".Res отключен администратором или другим плагином.";
            return false;
        }

        if (Round.Duration.TotalSeconds > 180) {
            response = "Уже прошло 180 секунд с начала раунда.";
            return false;
        }
        
        if (!PlayerCooldown.TryGet(player, out PlayerCooldown? cooldown))
            cooldown = new PlayerCooldown(player, TimeSpan.FromSeconds(10.0));
        
        if (cooldown!.Check()) {
            response = $"Вы не можете заспавнится ещё {cooldown.GetRemaining()} секунд.";
            return false;
        }
        
        if (player.Role != RoleTypeId.Spectator) {
            response = "Вы должны быть наблюдателем, чтобы использовать эту команду.";
            return false;
        }

        RoleTypeId role = UnityEngine.Random.Range(0, 100f) <= 75 ? RoleTypeId.ClassD : RoleTypeId.Scientist;
        
        Logger.Debug($"{player} used .res and became {role}");
            
        player.SetRole(role, RoleChangeReason.LateJoin);
            
        response = $"Вы успешно стали {(role == RoleTypeId.Scientist ? "<color=yellow>научным сотрудником" : "<color=orange>персоналом класса D")}</color>.";
        
        cooldown.Use();
        
        return true;
    }
}