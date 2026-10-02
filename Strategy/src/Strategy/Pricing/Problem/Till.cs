using dev.kaldiroglu.Strategy.Pricing.Domain;

namespace dev.kaldiroglu.Strategy.Pricing.Problem;

/// <summary>
/// The reversal: what stage three costs the moment the store asks for the obvious thing.
/// <para>
/// The promise the story opened with is that the customer is given the <em>best</em>
/// campaign they qualify for, and the receipt says what it saved them. Both need one basket
/// priced more than one way. In <c>Solution</c> that is a loop over rules. Here the campaign
/// is the object's class, so pricing a basket five ways means constructing five tills — and
/// this class, which is the caller, has to name every campaign in the company to do it.
/// </para>
/// <para>
/// <b>Read the field.</b> Adding a campaign now edits this file too, which is the thing
/// stage three was supposed to have fixed. The branch did not go away; it moved, and turned
/// into a list of type names.
/// </para>
/// </summary>
public sealed class Till
{
    // Every concrete campaign, named in the caller. GoF, p. 316: clients that choose an
    // algorithm this way are coupled to the class of every algorithm they might choose.
    private readonly IReadOnlyList<Checkout> _everyCampaign =
    [
        new PlainCheckout(),
        new StudentCheckout(),
        new BlackFridayCheckout()
    ];

    /// <summary>The best receipt this basket can be given, and the campaign that produced it.</summary>
    public Receipt BestFor(Basket basket)
    {
        var quotes = new List<Receipt>();
        foreach (var checkout in _everyCampaign)
        {
            quotes.Add(checkout.Ring(basket));
        }
        return quotes.MinBy(receipt => receipt.Paid)!;
    }

    /// <summary>How many campaign classes this caller had to know about to do that.</summary>
    public int CampaignsNamedHere => _everyCampaign.Count;
}
