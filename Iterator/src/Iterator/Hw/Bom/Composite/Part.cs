namespace dev.kaldiroglu.Iterator.Hw.Bom.Composite;

/// <summary>
/// Leaf role of the Composite pattern — a purchased part that is not broken down any further.
/// </summary>
/// <remarks>
/// <para>A rim, a spoke, a frame. It has a price from a supplier and a mass.</para>
/// <para>
/// Copied from the Composite port (<c>dev.kaldiroglu.Composite.Bom.Solution</c>), and trimmed
/// to what the Iterator homework needs. The pattern folders in this repository do not
/// reference each other, so the types are copied rather than shared. The roll-ups (cost,
/// weight, part count), the caches and the parent links are left out.
/// </para>
/// </remarks>
public sealed class Part : BomComponent
{
    public Part(string partNumber, string name, Money unitCost, int weightGrams)
        : base(partNumber, name)
    {
        if (weightGrams < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(weightGrams), weightGrams, "weight must not be negative");
        }

        UnitCost = unitCost;
        WeightGrams = weightGrams;
    }

    /// <summary>The supplier's price for one of these.</summary>
    public Money UnitCost { get; }

    /// <summary>The mass of one of these, in grams.</summary>
    public int WeightGrams { get; }
}
