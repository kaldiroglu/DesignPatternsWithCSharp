namespace dev.kaldiroglu.Strategy.Freight;

/// <summary>
/// A <b>ConcreteStrategy</b>: charge for the space the parcel takes, not what it weighs.
/// <para>
/// The domestic carriers rate on desi — volume over three thousand — and bill whichever of
/// desi and actual weight is greater, rounded up to the next unit. A pillow and a paving
/// slab cost the same to send if they fill the same box.
/// </para>
/// </summary>
public sealed class ByDesi : IRateCard
{
    private readonly Money _perUnit;
    private readonly int _minimumUnits;

    public ByDesi(string carrier, Money perUnit, int minimumUnits)
    {
        Carrier = carrier;
        _perUnit = perUnit;
        _minimumUnits = minimumUnits;
    }

    public string Carrier { get; }

    public Money Quote(Shipment shipment)
    {
        var units = Math.Max(_minimumUnits, CeilKilos(shipment.ChargeableGrams));
        return _perUnit.Times(units);
    }

    private static int CeilKilos(int grams) => (grams + 999) / 1000;
}
