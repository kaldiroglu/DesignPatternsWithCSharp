namespace dev.kaldiroglu.Decorator.Hw.PowerUps;

/// <summary>The <b>ConcreteComponent</b>: a fighter with no effects on them.</summary>
public sealed class Fighter : ICombatant
{
    private readonly int _baseDamage;

    public Fighter(int baseDamage) => _baseDamage = baseDamage;

    public int Damage() => _baseDamage;
}
