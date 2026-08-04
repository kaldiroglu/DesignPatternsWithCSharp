namespace dev.kaldiroglu.Composite.Gof.Equipment;

/// <summary>
/// Leaf role of the Composite pattern (GoF, p. 172) — a simple piece of
/// equipment.
/// </summary>
/// <remarks>
/// A disk drive answers the <see cref="Equipment"/> operations from its own
/// state: there are no children to consult.
/// </remarks>
public class FloppyDisk(string name, int watts, Currency listPrice) : Equipment(name)
{
    private const decimal DiscountRate = 0.10m; // 10% off list

    public FloppyDisk(string name) : this(name, 15, Currency.Of(35.00m))
    {
    }

    public override int Power() => watts;

    public override Currency NetPrice() => listPrice;

    public override Currency DiscountPrice() => listPrice.Times(1m - DiscountRate);
}

/// <summary>
/// Leaf role of the Composite pattern (GoF, p. 172) — a simple piece of
/// equipment: an expansion card that plugs into a <see cref="Bus"/>.
/// </summary>
public class Card(string name, int watts, Currency listPrice) : Equipment(name)
{
    private const decimal DiscountRate = 0.05m; // 5% off list

    public Card(string name) : this(name, 8, Currency.Of(120.00m))
    {
    }

    public override int Power() => watts;

    public override Currency NetPrice() => listPrice;

    public override Currency DiscountPrice() => listPrice.Times(1m - DiscountRate);
}
