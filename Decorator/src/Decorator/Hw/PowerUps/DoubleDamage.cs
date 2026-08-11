namespace dev.kaldiroglu.Decorator.Hw.PowerUps;

/// <summary>Twice whatever is beneath it.</summary>
public sealed class DoubleDamage : PowerUp
{
    public DoubleDamage(ICombatant component) : base(component)
    {
    }

    public override int Damage() => Component.Damage() * 2;
}
