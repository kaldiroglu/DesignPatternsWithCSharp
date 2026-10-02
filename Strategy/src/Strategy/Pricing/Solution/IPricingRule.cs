using dev.kaldiroglu.Strategy.Pricing.Domain;

namespace dev.kaldiroglu.Strategy.Pricing.Solution;

/// <summary>
/// The <b>Strategy</b>: one way of pricing a basket.
/// <para>
/// GoF, p. 315: "Define a family of algorithms, encapsulate each one, and make them
/// interchangeable. Strategy lets the algorithm vary independently from clients that use
/// it."
/// </para>
/// <para>
/// Two members, and both are about the <em>algorithm</em> rather than about the till. A rule
/// is handed a basket and answers what it charges; it never asks who is asking, never
/// decides whether it should be the rule in force, and never touches a receipt. That is what
/// makes one rule readable on its own page and testable without a checkout.
/// </para>
/// <para>
/// Note what is <em>not</em> here: no <c>AppliesTo</c>, no <c>Priority</c>, no
/// <c>IsBetterThan</c>. Choosing among rules is the context's business, and the moment a
/// rule starts ranking itself against the others, every rule has to know every other rule.
/// </para>
/// </summary>
public interface IPricingRule
{
    /// <summary>What to print above the total.</summary>
    string Name { get; }

    /// <summary>What this rule charges for the basket, whatever the shelf prices add up to.</summary>
    Money PriceFor(Basket basket);
}
