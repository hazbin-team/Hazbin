using CommandSystem;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;

namespace Hazbin.NoRules.Scp294;

[CommandHandler(typeof(ClientCommandHandler))]
public class DrinkCommand : ICommand {
    public string Command => "drink";
    public string[] Aliases => ["dr"];
    public string Description => "Выбрать напиток или узнать информацию о напитках";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
        if (Player.Get(sender) is not { } player) {
            response = "Вы не игрок.";
            return false;
        }

        if (arguments.Count <= 0) {
            response = "\nЧтобы выбрать напиток пропишите команду '.drink' и название напитка или его номер.";
            
            player.SendConsoleMessage("Доступные напитки: ", "white");
            player.SendConsoleMessage("\n" + string.Join("\n", 
                Scp294Plugin.Instance!._drinkMachine!.GetDrinks()
                    .Select(drink => $"[{Scp294Plugin.Instance._drinkMachine!.GetIdByDrink(drink)}] {drink.Name} (макс. {drink.Limit}) - {drink.Description}")
            ), "white");
            
            return true;
        }

        var drink = int.TryParse(arguments.At(0), out int id) ? Scp294Plugin.Instance!._drinkMachine!.GetDrinkById(id) : Scp294Plugin.Instance!._drinkMachine!.GetDrinkByName(arguments.At(0));
        
        Logger.Debug("Selected drink: " + drink.Name);
        
        Scp294Plugin.Instance._drinkMachine!.SelectDrink(player, drink);
        response = $"Вы выбрали {drink.Name}, Зажмите [E] по кофемашине чтобы получить напиток.";
        return true;
    }
}