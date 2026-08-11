namespace dev.kaldiroglu.Decorator.Hw.PowerUps;

/// <summary>
/// A decorator that <b>ignores what is beneath it</b> and returns a fixed number.
/// <para>
/// Structurally still a decorator — GoF's "zero, one or many" forwards allows zero. Worth
/// arguing about in the room: the pattern constrains structure, not honesty.
/// </para>
/// </summary>
public sealed class Berserk : PowerUp
{
    private readonly int _fixedDamage;

    public Berserk(ICombatant component, int fixedDamage) : base(component) =>
        _fixedDamage = fixedDamage;

    public override int Damage() => _fixedDamage;
}
