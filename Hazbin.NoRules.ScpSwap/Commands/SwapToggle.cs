using CommandSystem;
using LabApi.Features.Console;

namespace Hazbin.NoRules.ScpSwap.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]

public class SwapToggle : ICommand, IUsageProvider {
    public string Command => "ScpSwapToggle";

    public string[] Aliases { get; } = ["SwapToggle"];

    public string Description => "Включить/Выключить .swap (.sw)";

    public string[] Usage => ["Написав после команды \"Check\" вы узнаете включен-ли сейчас .swap"];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
        if (arguments.Count > 0 && arguments.At(0) == "Check") {
            response = $".swap (.sw) сейчас {(SwapPlugin.IsAllowed ? "включен" : "выключен")}.";
            return true;
        }

        SwapPlugin.IsAllowed = !SwapPlugin.IsAllowed;
            
        response = $".swap (.sw) теперь {(SwapPlugin.IsAllowed ? "включен" : "выключен")}.";
            
        Logger.Debug("Plugin.SwapAllowed is now " + SwapPlugin.IsAllowed);
            
        return true;
    }
    
}