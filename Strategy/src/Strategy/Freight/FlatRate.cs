namespace dev.kaldiroglu.Strategy.Freight;

/// <summary>
/// A <b>ConcreteStrategy</b>: one price, whatever it is and wherever it goes.
/// <para>
/// The card a marketplace negotiates for its sellers, and the one that shows a strategy may
/// ignore its input entirely and still be a strategy. It is also the card that wins most
/// often on small parcels and loses badly on large ones, which is the whole reason a
/// comparison exists.
/// </para>
/// </summary>
public sealed class FlatRate : IRateCard
{
    private readonly Money _price;

    public FlatRate(string carrier, Money price)
    {
        Carrier = carrier;
        _price = price;
    }

    public string Carrier { get; }

    public Money Quote(Shipment shipment) => _price;
}
