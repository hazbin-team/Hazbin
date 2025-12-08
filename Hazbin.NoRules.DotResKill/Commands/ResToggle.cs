using CommandSystem;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;

namespace Hazbin.NoRules.DotResKill.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal sealed class RespawnToggle : ICommand, IUsageProvider {
    public string Command => "RespawnToggle";

    public string[] Aliases { get; } = ["ResToggle"];

    public string Description => "Включить/Выключить .Respawn (.Res)";

    public string[] Usage => ["Написав после команды \"Check\" вы узнаете включен-ли сейчас .Res"];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
        if (Round.Duration.TotalSeconds > 180) {
            response = "Уже прошло 180 секунд с начала раунда.";
            return false;
        }

        if (arguments.Count > 0 && arguments.At(0) == "Check") {
            response = $".Respawn (.Res) сейчас {(DotPlugin.ResIsAllowed ? "включен" : "выключен")}.";
            return true;
        }

        DotPlugin.ResIsAllowed = !DotPlugin.ResIsAllowed;
            
        response = $".Respawn (.Res) теперь {(DotPlugin.ResIsAllowed ? "включен" : "выключен")}.";
            
        Logger.Debug("Plugin.ResAllowed is now " + DotPlugin.ResIsAllowed);
            
        return true;
    }
}