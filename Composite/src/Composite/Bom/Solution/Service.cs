using dev.kaldiroglu.Composite.Bom.Domain;

namespace dev.kaldiroglu.Composite.Bom.Solution;

/// <summary>
/// A second kind of Leaf, added to the design after everything else was written: a
/// subcontracted operation such as powder coating.
/// </summary>
/// <remarks>
/// <para>
/// It costs money, it adds no mass, and it is not a part anyone can put on a shelf —
/// so it answers the three roll-up questions differently from <see cref="Part"/>: a
/// fee, zero grams, and <b>zero</b> parts.
/// </para>
/// <para>
/// This class is the whole change. Nothing else in the namespace was touched, and no
/// client anywhere — costing, shipping, the tree printer, the roll-ups in
/// <see cref="Assembly"/> — needed a single edit. They were all written against
/// <see cref="BomComponent"/>, so they handled this class correctly before it existed.
/// </para>
/// <para>
/// That is GoF's third consequence (p. 166): <em>"Newly defined Composite or Leaf
/// subclasses work automatically with existing structures and existing client
/// code."</em> Compare <c>Problem.Service</c>, where the same requirement forces a
/// third collection on the assembly and a new branch in every client.
/// </para>
/// </remarks>
public sealed class Service : BomComponent
{
    public Service(string partNumber, string name, Money fee)
        : base(partNumber, name)
    {
        Fee = fee;
    }

    /// <summary>Creates a service from the shared reference data.</summary>
    public Service(Catalog.ServiceSpec spec)
        : this(spec.PartNumber, spec.Name, spec.Fee)
    {
    }

    /// <summary>What the subcontractor charges.</summary>
    public Money Fee { get; }

    public override Money TotalCost() => Fee;

    /// <summary>An operation adds no mass.</summary>
    public override int TotalWeightGrams() => 0;

    /// <summary>An operation is not a purchasable part, so it counts as none.</summary>
    public override int PartCount() => 0;
}
