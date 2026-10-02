using dev.kaldiroglu.Strategy.Pricing.Domain;

namespace dev.kaldiroglu.Strategy.Pricing.Solution;

/// <summary>
/// Every campaign the store is running today, and the answer to the question stage three
/// could not answer: which of them is best for this basket?
/// <para>
/// Compare with <c>Problem.Till</c>. That class had to name each campaign's type to price a
/// basket several ways. This one holds <see cref="IPricingRule"/>s and names none of them —
/// the list is handed in, so a campaign added on Thursday is a line of configuration rather
/// than an edit here.
/// </para>
/// <para>
/// This is also where GoF's implementation issue 1 (p. 319) is answered: somebody has to
/// decide which strategy is in force, and it should not be the strategies and it should not
/// be the context. Here it is a small object whose only job is choosing.
/// </para>
/// </summary>
public sealed class CampaignBook
{
    private readonly List<IPricingRule> _rules = [];

    public CampaignBook(params IPricingRule[] rules)
    {
        _rules.AddRange(rules);
    }

    /// <summary>Adding a campaign is one call, and no existing rule or till is touched.</summary>
    public CampaignBook Add(IPricingRule rule)
    {
        _rules.Add(rule);
        return this;
    }

    public int Size => _rules.Count;

    /// <summary>The rule that charges this basket least. Ties go to the rule registered first.</summary>
    public IPricingRule BestFor(Basket basket)
    {
        var best = _rules[0];
        foreach (var rule in _rules)
        {
            if (rule.PriceFor(basket).CompareTo(best.PriceFor(basket)) < 0)
            {
                best = rule;
            }
        }
        return best;
    }

    /// <summary>What every campaign would charge, for the "you could have saved more" panel.</summary>
    public IReadOnlyList<Receipt> QuoteAll(Basket basket)
    {
        var quotes = new List<Receipt>();
        var till = new Checkout(_rules[0]);
        foreach (var rule in _rules)
        {
            till.SetRule(rule);            // one till, every campaign
            quotes.Add(till.Ring(basket));
        }
        return [.. quotes];
    }
}
