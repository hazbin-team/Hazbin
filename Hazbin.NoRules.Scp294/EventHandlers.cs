using AdminToys;
using Exiled.API.Enums;
using Hazbin.Core.Extensions;
using Hazbin.Core.Features;
using Hazbin.NoRules.Scp294.EventArgs;
using Hazbin.NoRules.Scp294.Models;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using ProjectMER.Features;
using ProjectMER.Features.Objects;
using UnityEngine;
using PrimitiveObjectToy = LabApi.Features.Wrappers.PrimitiveObjectToy;
using Room = Exiled.API.Features.Room;

namespace Hazbin.NoRules.Scp294;

internal class EventHandlers(DrinkMachine dM) : CustomEventsHandler {
    // server
    public override void OnServerRoundStarted() {
        Room room = Room.Get(RoomType.EzPcs);
        
        SchematicObject schematic = ObjectSpawner.SpawnSchematic("294", room.Position, room.Rotation, Vector3.one);
        
        PrimitiveObjectToy parentToy = PrimitiveObjectToy.Create();
        parentToy.Type = PrimitiveType.Cube;
        parentToy.Flags = PrimitiveFlags.None;
        parentToy.GameObject.name = $"SCP294-{room.Name}-{Guid.NewGuid()}";
        parentToy.Position = schematic.transform.GetChild(0).position + new Vector3(0, 1f, 0);
        parentToy.Rotation = room.Rotation;
        parentToy.Scale = new Vector3(1.15809f, 2.49517f, 1.301302f);

        dM.Toy = InteractableToy.Create(parentToy.Transform);
        dM.Toy.GameObject.name = "scp500";
        dM.Toy.InteractionDuration = 5.05f;
        
        dM.Toy.OnSearched += this.OnSearched;
        dM.Toy.OnSearching += this.OnSearching;
        dM.Toy.OnSearchAborted += this.OnSearchAborted;
        
        dM.SoundPlayer = AudioPlayer.CreateOrGet("SCP294");
        dM.SoundPlayer.AddSpeaker("Main", dM.Toy.Parent!.position, 10f, true, 1f, 30f);
    }

    private void OnSearchAborted(LabApi.Features.Wrappers.Player obj) {
        if (obj.IsSCP) return;
        dM.SoundPlayer.RemoveClipByName("drink_machine");
    }

    private void OnSearched(LabApi.Features.Wrappers.Player player) {
        if (player.IsSCP) return;
        
        dM.ServeDrink(player);
    }

    private void OnSearching(LabApi.Features.Wrappers.Player player) {
        if (player.IsSCP) return;
        if (!PlayerData.Contains(player.UserId)) {
            Player.Get(player.UserId)!.ShowCoreHint("Вы не выбрали напиток! \nЧтобы выбрать напиток, в консоле на [~] пропишите команду '.drink'");
            return;
        }
        dM.SoundPlayer.AddClip("drink_machine");
    }
    
    // player
    public override void OnPlayerUsingItem(PlayerUsingItemEventArgs ev) {
        Drink? drink = dM.GetDrinks()
            .FirstOrDefault(d => d.Check(ev.UsableItem));

        if (drink == null) return;

        /*if (ev.Player.IsSubject()) {
            ev.IsAllowed = false;
            return;
        }*/
        
        drink.Drinking(new DrinkingEventArgs(drink, ev.Player, ev.UsableItem, ev.IsAllowed));
    }

    public override void OnPlayerUsedItem(PlayerUsedItemEventArgs ev) {
        Drink? drink = dM.GetDrinks()
            .FirstOrDefault(d => d.Check(ev.UsableItem));

        if (drink == null) return; 

        drink.Drinked(new DrinkedEventArgs(drink, ev.Player, ev.UsableItem));
    }

    public override void OnPlayerPickedUpItem(PlayerPickedUpItemEventArgs ev) {
        Drink? drink = dM.GetDrinks()
            .FirstOrDefault(d => d.Check(ev.Item));

        if (drink == null) return; 

        drink.OnPickedUpItem(ev);
    }

    public override void OnPlayerChangedItem(PlayerChangedItemEventArgs ev) {
        Drink? drink = dM.GetDrinks()
            .FirstOrDefault(d => d.Check(ev.NewItem));

        if (drink == null) return; 

        drink.OnChangedItem(ev);
    }
}