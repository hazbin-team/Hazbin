namespace Hazbin.Core.Features;

public class GlobalCooldown : IDisposable {
    public static HashSet<GlobalCooldown> Cooldowns { get; } = new();
    
    public object Owner { get; private set; }
    public TimeSpan CooldownTime { get; private set; }

    private DateTime _lastUse = DateTime.UtcNow;

    public GlobalCooldown(object owner, TimeSpan cooldownTime) {
        this.Owner = owner;
        this.CooldownTime = cooldownTime;
    }

    public void Use(bool overrideCooldown = false) {
        if (overrideCooldown) {
            this._lastUse = DateTime.UtcNow;
            return;
        }

        if (!this.Check())
            return;

        this._lastUse = DateTime.UtcNow;
    }

    public double GetRemaining() => (DateTime.UtcNow - (this._lastUse + this.CooldownTime)).TotalSeconds;
    
    public bool Check() => DateTime.UtcNow > this._lastUse + this.CooldownTime;

    public static GlobalCooldown? Get(object owner) => Cooldowns.FirstOrDefault(x => x.Owner == owner);

    public static HashSet<GlobalCooldown>? Get(TimeSpan cooldown) =>
        Cooldowns.Count(x => x.CooldownTime == cooldown) >= 1
            ? Cooldowns.Where(x => x.CooldownTime == cooldown).ToHashSet()
            : null;
    
    public static bool TryGet(object owner, out GlobalCooldown? cooldown) {
        cooldown = Get(owner);
        return cooldown != null;
    }

    public static bool TryGet(TimeSpan cooldown, out HashSet<GlobalCooldown>? cooldowns) {
        cooldowns = Get(cooldown);
        return cooldowns != null;
    }

    public void Dispose() {
        Cooldowns.Remove(this);
    }
}