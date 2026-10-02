namespace dev.kaldiroglu.Strategy.Freight;

/// <summary>
/// Every carrier under contract today, and the object that chooses between them.
/// <para>
/// The same shape as <c>Pricing.Solution.CampaignBook</c>, in a different domain, and for the
/// same reason: somebody has to decide which strategy is in force, and it should be neither
/// the strategies nor the context. A carrier signed on Monday is a line of configuration
/// here; a carrier dropped is a line removed. No rate card and no desk is touched either way.
/// </para>
/// <para>
/// Note what this class does <em>not</em> do: it never asks a card what kind of card it is.
/// It asks every one of them for a price and compares the numbers.
/// </para>
/// </summary>
public sealed class CarrierBoard
{
    private readonly List<IRateCard> _cards = [];

    public CarrierBoard(params IRateCard[] cards)
    {
        _cards.AddRange(cards);
    }

    public CarrierBoard Add(IRateCard card)
    {
        _cards.Add(card);
        return this;
    }

    public int Size => _cards.Count;

    /// <summary>What every carrier would charge, in the order they were registered.</summary>
    public IReadOnlyList<Quote> QuoteAll(Shipment shipment)
    {
        var quotes = new List<Quote>();
        var desk = new ShippingDesk(_cards[0]);
        foreach (var card in _cards)
        {
            desk.SetCard(card);              // one desk, every carrier
            quotes.Add(desk.Book(shipment));
        }
        return [.. quotes];
    }

    /// <summary>The cheapest quote. Ties go to the carrier registered first.</summary>
    public Quote CheapestFor(Shipment shipment) =>
        QuoteAll(shipment).MinBy(quote => quote.Price)!;
}
