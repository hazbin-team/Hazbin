using CommandSystem;
using LabApi.Features.Wrappers;
using PlayerRoles;
using ICommand = CommandSystem.ICommand;

namespace Hazbin.NoRules.ScpSwap.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class Swap : ICommand {
    public string Command => "swap";
    public string[] Aliases => ["sw"];
    public string Description => "Команда позволяющая свапать SCP на другово.";

    private static readonly Dictionary<string, RoleTypeId> Roles = new() {
        { "173", RoleTypeId.Scp173 },
        { "079", RoleTypeId.Scp079 },
        { "106", RoleTypeId.Scp106 },
        { "049", RoleTypeId.Scp049 },
        { "096", RoleTypeId.Scp096 },
        { "939", RoleTypeId.Scp939 }
    };
        
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
        Player.TryGet(sender, out Player? player);
        
        if (!SwapPlugin.IsAllowed) {
            response = ".swap отключен администратором или другим плагином.";
            return false;
        }

        if (Round.Duration.TotalSeconds >= 90) {
            response = "Прошло уже больше минуты с начала раунда!";
            return false;
        }

        if (!player!.IsSCP) {
            response = "Вы не являетесь SCP и не можете быть свапнуты!";
            return false;
        }

        if (arguments.Count < 1) {
            response = "Пиши команду правильно - .swap <SCP>";
            return false;
        }

        if (!Roles.TryGetValue(arguments.Array![1].ToLower(), out RoleTypeId role)) {
            response = "Такой SCP не найден в списке разрешённых!";
            return false;
        }
            
        int roleCount = Player.List.Count(x => x.Role == role);
            
        if (role == RoleTypeId.Scp079 && roleCount >= 1) {
            response = "SCP-079 может быть только один!";
            return false;
        }
            
        if (Player.List.Count < 30 && roleCount >= 1) {
            response = "Такой SCP уже есть в вашей команде!";
            return false;
        }
            
        if (Player.List.Count >= 30 && roleCount >= 2) {
            response = "Таких SCP уже слишком много!";
            return false;
        }
        
        player.SetRole(role, RoleChangeReason.RoundStart);

        response = "Успех! Вы изменили роль SCP на другую!";
        return true;
    }
}