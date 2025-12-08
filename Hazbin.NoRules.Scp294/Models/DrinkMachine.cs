using Exiled.API.Features;
using Hazbin.Core.Features;
using Hazbin.NoRules.Scp294.Drinks;
using LabApi.Features.Wrappers;
using Player = Exiled.API.Features.Player;

namespace Hazbin.NoRules.Scp294.Models;

public class DrinkMachine {
    private readonly List<Drink> _drinks = [
        new Water(),
        new Tea(),
        new Cola(),
        new Cum(),
        new Vodka(),
        new Pivo(),
        new Sniper(),
        new Scp207(),
        new Heal(),
        new AntiScp207(),
        new Master(),
        new Juice(),
        new Tyagi(),
        new Putin(),
        new Heaven(),
        new Meat(),
        new Shampoo(),
        new Baikal(),
        new Jaguar(),
        new Doping(),
        new LitEnergy()
    ];
        
    private TimeSpan Cooldown = TimeSpan.FromSeconds(30);
        
    internal InteractableToy Toy { get; set; }
    internal AudioPlayer SoundPlayer { get; set; }

    public List<Drink> GetDrinks() => this._drinks;
    
    internal void AddDrink(Drink drink) => this._drinks.Add(drink);
    
    public Drink GetDrinkByName(string name) => this._drinks.Find(drink => drink.Name.ToLower() == name.ToLower());
    public Drink GetDrinkById(int id) => this._drinks[id];

    public int GetIdByDrink(Drink drink) => this._drinks.IndexOf(drink);
    
    private bool CheckCooldown(Player player) {
        if (!PlayerData.TryGet(player.UserId, out PlayerData? playerData)) return true;
        if (!playerData!.Data.TryGetValue("drink", out List<object>? list)) return true;
        if (list.Count > 1 && list[1] is DateTime lastUse) {
            return DateTime.UtcNow - lastUse >= this.Cooldown;
        }
            
        return true;
    }
        
    public void SelectDrink(Player player, Drink drink) {
        if (PlayerData.Contains(player.UserId)) PlayerData.Remove(player.UserId);
        
        PlayerData.Add(player.UserId, "drink", [drink, 0]);
    }

    public void ServeDrink(Player player) {
        if (!PlayerData.TryGet(player.UserId, out PlayerData? playerData)) {
            player.ShowHint("Вы не выбрали напиток! \nЧтобы выбрать напиток, в консоле на [~] пропишите команду '.drink'");
            
            Log.Debug($"{nameof(this.ServeDrink)}: Player {player.Nickname} has no drink");
            
            return;
        }
            
        if (!this.CheckCooldown(player)) {
            Log.Debug($"{nameof(this.ServeDrink)}: Player {player.Nickname} is cooldowned");
            player.ShowHint("Кофемашина <b>перезагружается</b>! \nПодождите пару секунд.");
            
            return;
        }

        if (!playerData!.Data.TryGetValue("drink", out List<object>? list)) return;
        if (list?[0] is Drink drink && list[1] is int limit) {
            if (limit >= drink.Limit) {
                Log.Debug($"{nameof(this.ServeDrink)}: Player {player.Nickname} has reached the limit of {drink.Name}");
                player.ShowHint($"Кофемашина <b><color=red>больше не может</color> выдать вам {drink.Name}</b>!");
                return;
            }
            
            Log.Debug($"{nameof(this.ServeDrink)}: Giving {drink.Name} to {player.Nickname}");
            drink.Give(player);

            list[1] = limit + 1;

            if (list.Count <= 2) {
                list.Add(DateTime.UtcNow);
            }
                
            Log.Debug($"{nameof(this.ServeDrink)}: Giving {drink.Name} to {player.Nickname} done");
        }
    }
}