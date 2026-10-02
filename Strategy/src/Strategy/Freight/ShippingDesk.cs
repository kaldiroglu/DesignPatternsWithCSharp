namespace dev.kaldiroglu.Strategy.Freight;

/// <summary>
/// The <b>Context</b>: books a shipment with whichever carrier it has been given.
/// <para>
/// One field, and no branch. The desk does not know how the price was reached and cannot
/// find out — which is what lets a carrier be added, repriced or dropped without this class
/// being opened.
/// </para>
/// </summary>
public sealed class ShippingDesk
{
    private IRateCard _card;

    public ShippingDesk(IRateCard card)
    {
        _card = card ?? throw new ArgumentNullException(nameof(card), "a desk needs a carrier");
    }

    /// <summary>Contracts change. The desk does not.</summary>
    public void SetCard(IRateCard card)
    {
        ArgumentNullException.ThrowIfNull(card);
        _card = card;
    }

    public string Carrier => _card.Carrier;

    public Quote Book(Shipment shipment) => new(_card.Carrier, _card.Quote(shipment));
}
