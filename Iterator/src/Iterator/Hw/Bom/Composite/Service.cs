namespace dev.kaldiroglu.Iterator.Hw.Bom.Composite;

/// <summary>
/// A second kind of Leaf: a subcontracted operation such as assembly or powder coating.
/// </summary>
/// <remarks>
/// <para>It costs money, but it is not a part anyone can put on a shelf.</para>
/// <para>
/// Copied from the Composite port (<c>dev.kaldiroglu.Composite.Bom.Solution</c>), and trimmed
/// to what the Iterator homework needs. The pattern folders in this repository do not
/// reference each other, so the types are copied rather than shared. The roll-ups (cost,
/// weight, part count), the caches and the parent links are left out.
/// </para>
/// </remarks>
public sealed class Service(string partNumber, string name, Money fee) : BomComponent(partNumber, name)
{
    /// <summary>What the subcontractor charges.</summary>
    public Money Fee { get; } = fee;
}
