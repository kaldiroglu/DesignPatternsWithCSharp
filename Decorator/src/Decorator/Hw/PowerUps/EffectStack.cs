namespace dev.kaldiroglu.Decorator.Hw.PowerUps;

/// <summary>
/// The answer to the question the exercise really asks: <i>how do you take a decorator
/// back off a chain?</i>
/// <para>
/// You do not. A chain is built by nesting, so removing a link from the middle means
/// rebuilding it from the parts — and nobody holds the parts. So this class holds them:
/// it keeps the list of active effects and rebuilds the chain whenever it is asked. That
/// is GoF's liability, felt in your own hands rather than read off a slide.
/// </para>
/// </summary>
public sealed class EffectStack : ICombatant
{
    private readonly ICombatant _base;
    private readonly List<Effect> _active = [];
    private int _tick;

    public EffectStack(ICombatant baseCombatant) => _base = baseCombatant;

    public EffectStack Grant(Effect effect)
    {
        _active.Add(effect);
        return this;
    }

    public bool Revoke(string name) => _active.RemoveAll(e => e.Name == name) > 0;

    public EffectStack Advance(int ticks)
    {
        _tick += ticks;
        _active.RemoveAll(e => e.ExpiresAtTick <= _tick);
        return this;
    }

    public IReadOnlyList<string> ActiveEffects() => _active.Select(e => e.Name).ToList();

    /// <summary>Rebuilds the chain from the effects still active.</summary>
    public ICombatant Chain() => _active.Aggregate(_base, (current, effect) => effect.Apply(current));

    public int Damage() => Chain().Damage();
}
