using dev.kaldiroglu.Composite.Bom.Domain;

namespace dev.kaldiroglu.Composite.Bom.Solution;

/// <summary>
/// Leaf role of the Composite pattern — a purchased part that is not broken down
/// any further.
/// </summary>
/// <remarks>
/// A rim, a spoke, a bearing. It has a price from a supplier and a mass, and it
/// answers every <see cref="BomComponent"/> query from its own two fields. There
/// is no recursion here: this is where the recursion stops.
/// </remarks>
public sealed class Part : BomComponent
{
    private readonly int _weightGrams;

    public Part(string partNumber, string name, Money unitCost, int weightGrams)
        : base(partNumber, name)
    {
        if (weightGrams < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(weightGrams), weightGrams, "weight must not be negative");
        }

        UnitCost = unitCost;
        _weightGrams = weightGrams;
    }

    /// <summary>Creates a part from the shared reference data.</summary>
    public Part(Catalog.PartSpec spec)
        : this(spec.PartNumber, spec.Name, spec.UnitCost, spec.WeightGrams)
    {
    }

    /// <summary>The supplier's price for one of these.</summary>
    public Money UnitCost { get; }

    public override Money TotalCost() => UnitCost;

    public override int TotalWeightGrams() => _weightGrams;

    public override int PartCount() => 1;
}
