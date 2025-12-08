namespace Hazbin.NoRules.AutoBroadcast;

public class Config {
    public List<Broadcast> Broadcasts { get; set; } = [
        new() {
            Delay = 200.0f,
            Duration = 10,
            Message = "<b>Присоединяйтесь к нашему <color=red>адскому сообществу в дискорде!</color> (Escape - ServerInfo)</b>"
        },
        new() {
            Delay = 350.0f,
            Duration = 15,
            Message = "<b>Припиши к нику <color=yellow>#HAZBIN</color> и получай <color=orange>X2 опыта!</color></b>"
        }
    ];
}