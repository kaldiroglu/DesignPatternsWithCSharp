namespace dev.kaldiroglu.Decorator.Hw.PowerUps;

/// <summary>The <b>Decorator</b>: an effect wrapped around a combatant.</summary>
public abstract class PowerUp : ICombatant
{
    protected readonly ICombatant Component;

    protected PowerUp(ICombatant component) =>
        Component = component ?? throw new ArgumentNullException(nameof(component));

    public abstract int Damage();
}
