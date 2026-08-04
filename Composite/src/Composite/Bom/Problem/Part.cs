using dev.kaldiroglu.Composite.Bom.Domain;

namespace dev.kaldiroglu.Composite.Bom.Problem;

/// <summary>
/// The naive design's purchased part — a plain class with <b>no common base type</b>
/// shared with <see cref="Assembly"/>.
/// </summary>
/// <remarks>
/// <para>
/// Nothing here is wrong in itself. The class is small, clear and correct. The
/// damage is done by what is <em>missing</em>: because a <c>Part</c> and an
/// <see cref="Assembly"/> have no type in common, no client can be written against
/// "a thing in a bill of materials". Every client must ask which one it is holding.
/// </para>
/// <para>
/// Compare with <c>Solution.Part</c>, which is the same three fields — but derives
/// from <c>BomComponent</c>.
/// </para>
/// </remarks>
public class Part(string partNumber, string name, Money unitCost, int weightGrams)
{
    /// <summary>Creates a part from the shared reference data.</summary>
    public Part(Catalog.PartSpec spec)
        : this(spec.PartNumber, spec.Name, spec.UnitCost, spec.WeightGrams)
    {
    }

    public string PartNumber { get; } = partNumber;

    public string Name { get; } = name;

    public Money UnitCost { get; } = unitCost;

    public int WeightGrams { get; } = weightGrams;
}
