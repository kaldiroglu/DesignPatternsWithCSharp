namespace dev.kaldiroglu.Decorator.Hw.PowerUps;

/// <summary>Subtracts, never below zero.</summary>
public sealed class Poison : PowerUp
{
    private readonly int _severity;

    public Poison(ICombatant component, int severity) : base(component) => _severity = severity;

    public override int Damage() => Math.Max(0, Component.Damage() - _severity);
}
