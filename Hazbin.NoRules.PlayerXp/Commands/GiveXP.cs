using CommandSystem;
using Hazbin.Core.Features;

namespace Hazbin.NoRules.PlayerXp.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal sealed class GiveXP : PermittedCommand, IUsageProvider
{
    public override string Command => "givexp";

    public override string[] Aliases { get; } = ["gxp"];

    public override string Description => "Выдать игроку определённое количество опыта";
    public string[] Usage { get; } = ["user id", "amount"];
    protected override bool OnExecuted(ArraySegment<string> arguments, ICommandSender sender, out string response) {
        string userId = arguments.Array![1];

        if (!float.TryParse(arguments.Array[2], out float amount))
        {
            response = $"{arguments.Array[2]} не является типом float!";
            return false;
        }
        
        XpPlugin.Instance!._database!.GiveXp(userId, amount);
        response = $"Выдано {userId} {amount} опыта";
        return true;
    }
}