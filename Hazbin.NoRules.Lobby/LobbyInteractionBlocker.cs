using InventorySystem.Items;

namespace Hazbin.NoRules.Lobby;

public class LobbyInteractionBlocker : IInteractionBlocker {
    public BlockedInteraction BlockedInteractions => BlockedInteraction.GeneralInteractions |
                                                     BlockedInteraction.GrabItems | 
                                                     BlockedInteraction.OpenInventory | 
                                                     BlockedInteraction.UndisarmPlayers |
                                                     BlockedInteraction.BeDisarmed |
                                                     BlockedInteraction.OpenInventory;

    public bool CanBeCleared => false;
}