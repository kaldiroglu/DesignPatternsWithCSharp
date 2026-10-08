namespace dev.kaldiroglu.Iterator.Hw.Bom.Composite;

/// <summary>
/// Composite role of the Composite pattern — a sub-assembly or a finished product.
/// </summary>
/// <remarks>
/// <para>
/// An assembly has a list of <see cref="BomLine"/>s naming what goes into it. The lines may
/// point at parts, services or other assemblies.
/// </para>
/// <para>
/// Copied from the Composite port (<c>dev.kaldiroglu.Composite.Bom.Solution</c>), and trimmed
/// to what the Iterator homework needs. The pattern folders in this repository do not
/// reference each other, so the types are copied rather than shared. The roll-ups (cost,
/// weight, part count), the caches and the parent links are left out.
/// </para>
/// </remarks>
public sealed class Assembly(string partNumber, string name) : BomComponent(partNumber, name)
{
    private readonly List<BomLine> _lines = [];

    /// <summary>Adds <paramref name="quantity"/> of <paramref name="component"/> to this assembly.</summary>
    /// <returns>This assembly, so lines can be chained while building a product.</returns>
    public Assembly Add(BomComponent component, int quantity)
    {
        _lines.Add(new BomLine(component, quantity));
        return this;
    }

    /// <summary>Adds exactly one of <paramref name="component"/> to this assembly.</summary>
    public Assembly Add(BomComponent component) => Add(component, 1);

    public override IReadOnlyList<BomLine> Lines => _lines.AsReadOnly();
}
