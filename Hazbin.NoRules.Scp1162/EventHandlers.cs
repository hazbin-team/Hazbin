using AdminToys;
using Exiled.API.Enums;
using Hazbin.Core.Extensions;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using UnityEngine;
using PrimitiveObjectToy = LabApi.Features.Wrappers.PrimitiveObjectToy;
using Room = Exiled.API.Features.Room;
using Player = Exiled.API.Features.Player;
using Random = UnityEngine.Random;

namespace Hazbin.NoRules.Scp1162;

internal class EventHandlers(List<ItemType> allowedItems) : CustomEventsHandler {
    public override void OnServerRoundStarted() {
        Timing.CallDelayed(2.5f, () => {
            Room room = Room.Get(RoomType.Lcz173);

            PrimitiveObjectToy parentToy = PrimitiveObjectToy.Create();
            parentToy.Type = PrimitiveType.Cylinder;
            parentToy.Flags = PrimitiveFlags.Visible;
            parentToy.GameObject.name = $"SCP1162-{room.Name}-{Guid.NewGuid()}";
            parentToy.Position = room.Position + room.Rotation * new Vector3(16.846f, 13, 3.689f);
            parentToy.Rotation = Quaternion.Euler(90, 0, 0);
            parentToy.Scale = new Vector3(1f, 0.05f, 1f);
            parentToy.Color = Color.black;
            
            InteractableToy toy = InteractableToy.Create(parentToy.Transform);
            toy.GameObject.name = "coin";
        
            toy.OnInteracted += this.OnInteracted;
        });
    }

    private void OnInteracted(LabApi.Features.Wrappers.Player ply) {
        Player player = Player.Get(ply.UserId);
        
        //if (player.IsSubject() && player.IsScp) return;
            
        if ((player?.CurrentItem?.Type ?? ItemType.None) == ItemType.None) {
            player!.EnableEffect(EffectType.Flashed, 1, 1);
            player.EnableEffect(EffectType.SeveredHands);
            player.EnableEffect(EffectType.Traumatized);

            player.Health -= 30;

            ply.ShowCoreHint("<b>Вы протянули <color=red>пустую</color> руку но <color=red>не смогли её вытянуть</color></b>");
        }
        else {
            if (Random.Range(0, 100) < 5) {
                player!.EnableEffect(EffectType.Flashed, 1, 1);
                player.EnableEffect(EffectType.SeveredHands);
                player.EnableEffect(EffectType.Traumatized);

                player.Health -= 30;

                ply.ShowCoreHint("<b>Вы протянули руку но <color=red>не смогли её вытянуть</color></b>");
                return;
            }
            
            player!.RemoveHeldItem();

            ItemType randomItem = allowedItems.RandomItem();
            
            ply.ShowCoreHint($"<b>Вы протянули руку и вытянули {randomItem.TranslateItemType(true)}</b>");

            player.CurrentItem = player.AddItem(randomItem);
        }
    }
}