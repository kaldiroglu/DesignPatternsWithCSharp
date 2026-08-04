namespace dev.kaldiroglu.Composite.Gof.Equipment;

/// <summary>
/// Composite role of the Composite pattern — the <c>CompositeEquipment</c> class
/// of the GoF Sample Code (GoF, p. 171).
/// </summary>
/// <remarks>
/// <para>
/// The base class for equipment that <em>contains</em> other equipment. It holds
/// the child list and implements each <see cref="Equipment"/> operation by
/// iterating over the children and combining their answers. Concrete assemblies
/// (<see cref="Chassis"/>, <see cref="Cabinet"/>, <see cref="Bus"/>) inherit that
/// behavior and add their own contribution on top of it.
/// </para>
/// <para>
/// This is where the pattern earns its keep. Note that <see cref="NetPrice"/>
/// does not care whether a child is a card or another whole chassis — the
/// recursion terminates by itself when it reaches leaves.
/// </para>
/// </remarks>
public abstract class CompositeEquipment(string name) : Equipment(name)
{
    private readonly List<Equipment> _equipment = [];

    /// <summary>
    /// The power drawn by the assembly's own circuitry, excluding its contents.
    /// Subclasses override it; by default an enclosure consumes nothing itself.
    /// </summary>
    protected virtual int OwnPower => 0;

    /// <summary>The price of the assembly's own hardware, excluding its contents.</summary>
    protected virtual Currency OwnNetPrice => Currency.Zero;

    /// <summary>The discount rate applied to the assembly's own hardware.</summary>
    protected virtual decimal OwnDiscountRate => 0m;

    public override int Power()
    {
        var total = OwnPower;
        foreach (var part in _equipment)
        {
            total += part.Power(); // may recurse into another assembly
        }

        return total;
    }

    public override Currency NetPrice()
    {
        var total = OwnNetPrice;
        foreach (var part in _equipment)
        {
            total = total.Plus(part.NetPrice());
        }

        return total;
    }

    public override Currency DiscountPrice()
    {
        var total = OwnNetPrice.Times(1m - OwnDiscountRate);
        foreach (var part in _equipment)
        {
            total = total.Plus(part.DiscountPrice());
        }

        return total;
    }

    public override void Add(Equipment part) => _equipment.Add(part);

    public override void Remove(Equipment part) => _equipment.Remove(part);

    public override IEnumerator<Equipment> GetEnumerator() => _equipment.GetEnumerator();

    public override bool IsComposite => true;

    /// <summary>The directly contained equipment, as a read-only list.</summary>
    public IReadOnlyList<Equipment> Parts => _equipment.AsReadOnly();
}
