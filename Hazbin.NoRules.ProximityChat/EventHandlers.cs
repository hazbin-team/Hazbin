using Exiled.Events.EventArgs.Player;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.Spectating;
using PlayerRoles.Voice;
using VoiceChat;
using VoiceChat.Networking;
using Vector3 = UnityEngine.Vector3;

namespace Hazbin.NoRules.ProximityChat;

public class EventHandlers : CustomEventsHandler {
    private static readonly HashSet<Player> ToggledPlayers = [];

    public override void OnServerRoundRestarted() {
        ToggledPlayers.Clear();
    }

    public override void OnPlayerTogglingNoclip(PlayerTogglingNoclipEventArgs ev) {
        if (FpcNoclip.IsPermitted(ev.Player.ReferenceHub))
            return;
        
        if (!ChatPlugin.Instance!.Config.AllowedRoles.Contains(ev.Player.Role))
            return;
        
        if (!ToggledPlayers.Add(ev.Player))
        {
            ToggledPlayers.Remove(ev.Player);
            //TODO: Вызвать в худе метод отключения проксимити чата
            Hud.EventHandlers.SetScpStatus(ev.Player, 0);
            ev.IsAllowed = false;
            return;
        }

        //TODO: Вызвать в худе метод включения проксимити чата
        Hud.EventHandlers.SetScpStatus(ev.Player, 1);
        
        ev.IsAllowed = false;
    }
    
    
    public static void OnPlayerUsingVoiceChat(VoiceChattingEventArgs ev)
    {
        if (ev.VoiceMessage.Channel != VoiceChatChannel.ScpChat)
            return;
        
        if (!ChatPlugin.Instance!.Config.AllowedRoles.Contains(ev.Player.Role.Type) || !ToggledPlayers.Contains(ev.Player))
            return;
            
        SendProximityMessage(ev.VoiceMessage);
        
        ev.IsAllowed = false;
    }
    
    private static void SendProximityMessage(VoiceMessage msg)
    {
        foreach (ReferenceHub referenceHub in ReferenceHub.AllHubs)
        {
            if (referenceHub.roleManager.CurrentRole is SpectatorRole && !msg.Speaker.IsSpectatedBy(referenceHub))
                continue;
                
            if (referenceHub.roleManager.CurrentRole is not IVoiceRole voiceRole2)
                continue;
            
            if (Vector3.Distance(msg.Speaker.transform.position, referenceHub.transform.position) >= 7.0f)
                continue;

            if (voiceRole2.VoiceModule.ValidateReceive(msg.Speaker, VoiceChatChannel.Proximity) is VoiceChatChannel.None)
                continue;
            
            msg.Channel = VoiceChatChannel.Proximity;
            referenceHub.connectionToClient.Send(msg);
        }
    }
}