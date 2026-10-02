namespace dev.kaldiroglu.Strategy.Freight;

/// <summary>
/// A <b>ConcreteStrategy</b>: a base price for the route, plus a rate for every kilo.
/// <para>
/// The international carriers price the distance first and the parcel second, and then add
/// a fuel surcharge that changes with the oil price rather than with anything about the
/// shipment. The surcharge is the reason this card holds state the others do not — it is
/// updated by a feed, not by a contract.
/// </para>
/// </summary>
public sealed class ByZone : IRateCard
{
    private readonly IReadOnlyDictionary<string, Money> _zoneBase;
    private readonly Money _perKilo;
    private int _fuelSurchargePercent;

    public ByZone(string carrier, IReadOnlyDictionary<string, Money> zoneBase, Money perKilo,
                  int fuelSurchargePercent)
    {
        Carrier = carrier;
        _zoneBase = new Dictionary<string, Money>(zoneBase);
        _perKilo = perKilo;
        _fuelSurchargePercent = fuelSurchargePercent;
    }

    /// <summary>The feed moved. No shipment, no quote and no other carrier is affected.</summary>
    public void SetFuelSurchargePercent(int percent)
    {
        _fuelSurchargePercent = percent;
    }

    public string Carrier { get; }

    public Money Quote(Shipment shipment)
    {
        if (!_zoneBase.TryGetValue(shipment.ToZone, out var @base))
        {
            throw new ArgumentException(Carrier + " does not serve " + shipment.ToZone, nameof(shipment));
        }
        var kilos = (shipment.ChargeableGrams + 999) / 1000;
        return @base.Plus(_perKilo.Times(kilos)).PercentMore(_fuelSurchargePercent);
    }
}
