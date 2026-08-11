namespace dev.kaldiroglu.Decorator.Hw.PowerUps;

/// <summary>An effect, its expiry, and how it wraps a combatant.</summary>
public sealed record Effect(string Name, int ExpiresAtTick, Func<ICombatant, ICombatant> Apply)
{
    public static Effect DoubleDamage(string name, int expiresAtTick) =>
        new(name, expiresAtTick, c => new DoubleDamage(c));

    public static Effect Poison(string name, int expiresAtTick, int severity) =>
        new(name, expiresAtTick, c => new Poison(c, severity));

    public static Effect Berserk(string name, int expiresAtTick, int fixedDamage) =>
        new(name, expiresAtTick, c => new Berserk(c, fixedDamage));
}
