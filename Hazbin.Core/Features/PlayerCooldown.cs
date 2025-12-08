namespace Hazbin.Core.Features;

public class PlayerCooldown : GlobalCooldown {
    public Player? Player { get; private set; }
    
    public PlayerCooldown(object owner, TimeSpan cooldownTime) : base(owner, cooldownTime) {
        if (owner is not Player player)
            throw new InvalidCastException($"{owner} cannot be casted to Player");

        this.Player = player;
    }

    public static PlayerCooldown? Get(Player player) {
        GlobalCooldown? cooldown = Get((object)player);

        return cooldown as PlayerCooldown;
    }

    public static bool TryGet(Player player, out PlayerCooldown? cooldown) {
        cooldown = Get(player);
        return cooldown != null;
    }
}