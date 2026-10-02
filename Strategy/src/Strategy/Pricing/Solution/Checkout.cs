using dev.kaldiroglu.Strategy.Pricing.Domain;

namespace dev.kaldiroglu.Strategy.Pricing.Solution;

/// <summary>
/// The <b>Context</b>: a till, which holds a rule and does not know which one.
/// <para>
/// One field, and it is the whole pattern. The till does not extend a campaign and does not
/// contain a branch over campaigns; it is handed one and asks it for a price. GoF, p. 316:
/// the context forwards requests from its clients to its strategy, and clients usually hand
/// the context the strategy they want.
/// </para>
/// <para>
/// <b>The rule can be replaced on a till that already exists.</b> That is the operation
/// stage three could not perform: there the campaign was the object's class, so the
/// Thursday change meant a different object and a caller that named it.
/// </para>
/// </summary>
public sealed class Checkout
{
    private IPricingRule _rule;

    public Checkout(IPricingRule rule)
    {
        _rule = rule ?? throw new ArgumentNullException(nameof(rule), "a till needs a rule; use ShelfPrice for none");
    }

    /// <summary>Point this till at a different campaign. The till itself does not change.</summary>
    public void SetRule(IPricingRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule);
        _rule = rule;
    }

    public string RuleName => _rule.Name;

    /// <summary>
    /// Price one basket and hand back the receipt.
    /// <para>
    /// Read what is not here: no <c>switch</c>, no type test, and no campaign name anywhere
    /// in the method. The saving is the shelf price less what the rule charged, which is why
    /// the receipt's promise costs nothing to keep.
    /// </para>
    /// </summary>
    public Receipt Ring(Basket basket) =>
        new(_rule.Name, basket.ListTotal, _rule.PriceFor(basket));
}
