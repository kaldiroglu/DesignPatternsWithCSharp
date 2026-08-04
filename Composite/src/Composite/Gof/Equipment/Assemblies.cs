namespace dev.kaldiroglu.Composite.Gof.Equipment;

/// <summary>
/// A concrete Composite (GoF, p. 172): the chassis that holds buses and drives.
/// </summary>
/// <remarks>
/// The book's <c>Chassis::NetPrice</c> adds the chassis's own price to the
/// aggregate computed by <c>CompositeEquipment::NetPrice</c>. Here that is
/// expressed by overriding the <c>Own...</c> hooks rather than the aggregate
/// operations, so the summing loop lives in exactly one place.
/// </remarks>
public class Chassis(string name, int watts, Currency listPrice)
    : CompositeEquipment(name)
{
    public Chassis(string name) : this(name, 25, Currency.Of(210.00m))
    {
    }

    protected override int OwnPower => watts;

    protected override Currency OwnNetPrice => listPrice;

    protected override decimal OwnDiscountRate => 0.15m; // 15% off list on the chassis itself
}

/// <summary>
/// A concrete Composite (GoF, p. 172): the cabinet that holds chassis.
/// </summary>
/// <remarks>
/// A cabinet is a passive enclosure — it consumes no power of its own — which
/// shows that a composite need not contribute anything to the aggregate. Its
/// whole job can be to hold children.
/// </remarks>
public class Cabinet(string name, Currency listPrice) : CompositeEquipment(name)
{
    public Cabinet(string name) : this(name, Currency.Of(90.00m))
    {
    }

    protected override Currency OwnNetPrice => listPrice;

    protected override decimal OwnDiscountRate => 0.20m; // 20% off list on the empty cabinet
}

/// <summary>
/// A concrete Composite (GoF, p. 172): the bus that holds expansion cards.
/// </summary>
/// <remarks>
/// A <c>Bus</c> is a composite that is itself contained by a
/// <see cref="Chassis"/>. It is the middle level of the example's tree, and the
/// clearest illustration that "composite" is a role an object plays, not a place
/// in a hierarchy of classes: the same object is a child to its parent and a
/// parent to its children.
/// </remarks>
public class Bus(string name, int watts, Currency listPrice) : CompositeEquipment(name)
{
    public Bus(string name) : this(name, 10, Currency.Of(75.00m))
    {
    }

    protected override int OwnPower => watts;

    protected override Currency OwnNetPrice => listPrice;

    protected override decimal OwnDiscountRate => 0.10m; // 10% off list on the bus itself
}
