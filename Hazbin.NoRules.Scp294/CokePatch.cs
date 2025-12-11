using HarmonyLib;
using Hazbin.NoRules.Scp294.Models;
using InventorySystem.Items.Usables;
using LabApi.Features.Wrappers;

namespace Hazbin.NoRules.Scp294;

[HarmonyPatch(typeof(AntiScp207), nameof(AntiScp207.OnEffectsActivated))]
internal static class CokePatch {
    private static bool Prefix(AntiScp207 __instance) {
        Drink? drink = Scp294Plugin.Instance!._drinkMachine!.GetDrinks()
            .FirstOrDefault(d => d.Check(Player.Get(__instance.Owner).CurrentItem));

        if (drink == null) return true; 
        
        return false;
    }
}