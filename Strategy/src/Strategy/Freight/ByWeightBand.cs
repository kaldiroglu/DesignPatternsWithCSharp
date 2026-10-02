namespace dev.kaldiroglu.Strategy.Freight;

/// <summary>
/// A <b>ConcreteStrategy</b>: a printed table of weight bands, and a price for each.
/// <para>
/// Not a rate at all — a lookup. The price does not rise smoothly with weight; it steps, and
/// a parcel one gram over a band edge costs a whole band more. This is the card that would
/// not survive an interface shaped as <c>Money PerKilo()</c>.
/// </para>
/// </summary>
public sealed class ByWeightBand : IRateCard
{
    /// <summary>
    /// One row of the table.
    /// </summary>
    /// <param name="UpToGrams">the top of the band, inclusive</param>
    /// <param name="Price">what anything in this band costs</param>
    public sealed record Band(int UpToGrams, Money Price);

    private readonly IReadOnlyList<Band> _bands;
    private readonly Money _overflowPrice;

    public ByWeightBand(string carrier, IReadOnlyList<Band> bands, Money overflowPrice)
    {
        Carrier = carrier;
        _bands = [.. bands.OrderBy(band => band.UpToGrams)];
        _overflowPrice = overflowPrice;
    }

    public string Carrier { get; }

    public Money Quote(Shipment shipment)
    {
        var grams = shipment.ChargeableGrams;
        foreach (var band in _bands)
        {
            if (grams <= band.UpToGrams)
            {
                return band.Price;
            }
        }
        return _overflowPrice;
    }
}
