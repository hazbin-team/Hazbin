using AdminToys;
using Hazbin.Core.Enums;
using Hazbin.Core.Extensions;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using UnityEngine;
using PrimitiveObjectToy = LabApi.Features.Wrappers.PrimitiveObjectToy;
using Random = UnityEngine.Random;

namespace Hazbin.NoRules.Scp1162;

internal class EventHandlers(List<ItemType> allowedItems) : CustomEventsHandler {
    public override void OnServerRoundStarted() {
        Timing.CallDelayed(2.5f, () => {
            Room room = Room.Get(RoomName.Lcz173).First();

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

    private void OnInteracted(Player ply) {
        //if (player.IsSubject() && player.IsScp) return;
            
        if ((ply?.CurrentItem?.Type ?? ItemType.None) == ItemType.None) {
            ply!.EnableEffect(EffectType.Flashed, 1, 1);
            ply.EnableEffect(EffectType.SeveredHands);
            ply.EnableEffect(EffectType.Traumatized);

            ply.Health -= 30;

            ply.ShowCoreHint("<b>Вы протянули <color=red>пустую</color> руку но <color=red>не смогли её вытянуть</color></b>");
        }
        else {
            if (Random.Range(0, 100) < 5) {
                ply!.EnableEffect(EffectType.Flashed, 1, 1);
                ply.EnableEffect(EffectType.SeveredHands);
                ply.EnableEffect(EffectType.Traumatized);

                ply.Health -= 30;

                ply.ShowCoreHint("<b>Вы протянули руку но <color=red>не смогли её вытянуть</color></b>");
                return;
            }
            
            ply!.RemoveItem(ply.CurrentItem!);

            ItemType randomItem = allowedItems.RandomItem();
            
            ply.ShowCoreHint($"<b>Вы протянули руку и вытянули {randomItem.TranslateItemType(true)}</b>");

            ply.CurrentItem = ply.AddItem(randomItem);
        }
    }
}