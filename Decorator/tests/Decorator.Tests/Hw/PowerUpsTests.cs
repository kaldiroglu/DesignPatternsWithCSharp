using dev.kaldiroglu.Decorator.Hw.PowerUps;
using Xunit;

namespace dev.kaldiroglu.Decorator.Tests.Hw;

/// <summary>
/// Homework 3, the one with no clean answer — which is why it is here. Decorator has no
/// removal story, so <see cref="EffectStack"/> keeps the parts and rebuilds the chain.
/// </summary>
public class PowerUpsTests
{
    [Fact(DisplayName = "effects stack, and their order changes the damage")]
    public void OrderChangesTheDamage()
    {
        var doubleThenPoison = new Poison(new DoubleDamage(new Fighter(10)), 5);
        var poisonThenDouble = new DoubleDamage(new Poison(new Fighter(10), 5));

        Assert.Equal(15, doubleThenPoison.Damage()); // (10 * 2) - 5
        Assert.Equal(10, poisonThenDouble.Damage()); // (10 - 5) * 2
    }

    [Fact(DisplayName = "a decorator that forwards zero times is still a decorator")]
    public void BerserkIgnoresWhatIsBeneathIt() =>
        Assert.Equal(50, new Berserk(new Poison(new DoubleDamage(new Fighter(10)), 5), 50).Damage());

    [Fact(DisplayName = "poison never takes damage below zero")]
    public void PoisonIsClamped() => Assert.Equal(0, new Poison(new Fighter(3), 10).Damage());

    [Fact(DisplayName = "an effect can be revoked, because the stack kept the parts")]
    public void RevokeRebuildsTheChain()
    {
        var stack = new EffectStack(new Fighter(10))
            .Grant(Effect.DoubleDamage("rage", 100))
            .Grant(Effect.Poison("venom", 100, 5));

        Assert.Equal(15, stack.Damage()); // (10 * 2) - 5

        Assert.True(stack.Revoke("venom"));
        Assert.Equal(20, stack.Damage()); // the chain was rebuilt without it
        Assert.Equal(["rage"], stack.ActiveEffects());
    }

    [Fact(DisplayName = "effects expire on their own tick")]
    public void EffectsExpire()
    {
        var stack = new EffectStack(new Fighter(10))
            .Grant(Effect.DoubleDamage("rage", 3))
            .Grant(Effect.Berserk("frenzy", 10, 50));

        Assert.Equal(50, stack.Damage()); // berserk is outermost and ignores the rest

        stack.Advance(3);                 // rage expires
        Assert.Equal(["frenzy"], stack.ActiveEffects());

        stack.Advance(7);                 // and so does frenzy
        Assert.Empty(stack.ActiveEffects());
        Assert.Equal(10, stack.Damage()); // back to the bare fighter
    }

    [Fact(DisplayName = "revoking something that was never granted says so")]
    public void RevokingAnAbsentEffect() =>
        Assert.False(new EffectStack(new Fighter(10)).Revoke("nothing"));
}
