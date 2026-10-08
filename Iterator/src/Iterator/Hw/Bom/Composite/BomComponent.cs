namespace dev.kaldiroglu.Iterator.Hw.Bom.Composite;

/// <summary>
/// Component role of the Composite pattern — one entry in a bill of materials.
/// </summary>
/// <remarks>
/// <para>
/// A bill of materials describes what a manufactured product is made of. A bicycle contains
/// wheels; a wheel contains a rim and spokes. The nesting has no fixed depth.
/// </para>
/// <para>
/// Copied from the Composite port (<c>dev.kaldiroglu.Composite.Bom.Solution</c>), and trimmed
/// to what the Iterator homework needs. The pattern folders in this repository do not
/// reference each other, so the types are copied rather than shared. The roll-ups (cost,
/// weight, part count), the caches and the parent links are left out.
/// </para>
/// </remarks>
public abstract class BomComponent(string partNumber, string name)
{
    /// <summary>The catalog identifier, e.g. <c>"RIM-700C"</c>.</summary>
    public string PartNumber { get; } = partNumber;

    /// <summary>The human-readable name, e.g. <c>"rim"</c>.</summary>
    public string Name { get; } = name;

    /// <summary>The lines that make up this component — empty for a part or a service.</summary>
    public virtual IReadOnlyList<BomLine> Lines => [];
}
