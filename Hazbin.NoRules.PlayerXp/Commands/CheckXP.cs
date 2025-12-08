using CommandSystem;
using Hazbin.Core.Features;

namespace Hazbin.NoRules.PlayerXp.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal sealed class CheckXP : PermittedCommand, IUsageProvider
{
    public override string Command => "checkxp";

    public override string[] Aliases { get; } = ["cxp"];

    public override string Description => "Узнать количество опыта у определённого игрока";
    public string[] Usage { get; } = ["user id"];
    protected override bool OnExecuted(ArraySegment<string> arguments, ICommandSender sender, out string response) {
        string userId = arguments.Array![1];
        float amount = XpPlugin.Instance!._database!.GetXp(userId);
        
        response = $"У игрока {userId} {amount} опыта";
        return true;
    }
}