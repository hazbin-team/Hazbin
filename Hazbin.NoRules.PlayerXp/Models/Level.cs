using Hazbin.Core.Enums;

namespace Hazbin.NoRules.PlayerXp.Models;

public class Level
{
    public float Xp { get; set; }
    public string Text { get; set; } = null!;
    public CustomInfoColor Color { get; set; }
}