using CommandSystem;
using Hazbin.Core.Features;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using PlayerRoles;
using ICommand = CommandSystem.ICommand;

namespace Hazbin.NoRules.DotResKill.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
internal sealed class Kill : ICommand {
    public string Command => "killme";

    public string[] Aliases { get; } = ["kill"];

    public string Description => "Убейте себя(в игре конечно).";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
        Player? player = Player.Get(sender);
        if (player == null) {
            response = "Вы не являетесь игроком. Скорее всего вы пишите команду в консоли сервера.";
            return false;
        }
        
        if (!DotPlugin.KillIsAllowed) {
            response = ".kill отключен администратором или другим плагином.";
            return false;
        }
        
        if (player.Role == RoleTypeId.Spectator) {
            response = "Вы должны быть живы, чтобы использовать эту команду.";
            return false;
        }

        if (!PlayerCooldown.TryGet(player, out PlayerCooldown? cooldown))
            cooldown = new PlayerCooldown(player, TimeSpan.FromSeconds(10.0));
        
        if (cooldown!.Check()) {
            response = $"Вы не можете умереть ещё {cooldown.GetRemaining()} секунд.";
            return false;
        }
            
        Logger.Debug($"{player} used .kill");
        
        player.Kill(DotPlugin.Instance!.Config.KillReasons.RandomItem());
            
        response = "Суицид <color=red>не</color> выход.";
        
        cooldown.Use();
        
        return true;
    }
}