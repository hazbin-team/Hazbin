using CommandSystem;
using LabApi.Features.Console;

namespace Hazbin.NoRules.DotResKill.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal sealed class KillToggle : ICommand, IUsageProvider {
    public string Command => "KillmeToggle";

    public string[] Aliases { get; } = ["KillToggle"];

    public string Description => "Включить/Выключить .Killme (.Kill)";

    public string[] Usage => ["Написав после команды \"Check\" вы узнаете включен-ли сейчас .Kill"];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
        if (arguments.Count > 0 && arguments.At(0) == "Check") {
            response = $".Killme (.Kill) сейчас {(DotPlugin.KillIsAllowed ? "включен" : "выключен")}.";
            return true;
        }

        DotPlugin.KillIsAllowed = !DotPlugin.KillIsAllowed;
            
        response = $".Killme (.Kill) теперь {(DotPlugin.KillIsAllowed ? "включен" : "выключен")}.";
        
        Logger.Debug("Plugin.KillAllowed is now " + DotPlugin.KillIsAllowed);
            
        return true;
    }
}