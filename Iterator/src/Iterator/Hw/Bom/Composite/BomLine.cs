namespace dev.kaldiroglu.Iterator.Hw.Bom.Composite;

/// <summary>
/// One line of a bill of materials: a component and how many of it the parent assembly
/// requires.
/// </summary>
/// <remarks>
/// <para>
/// The quantity is on the line, not on the component, so one component can be used by
/// several parents.
/// </para>
/// <para>
/// Copied from the Composite port (<c>dev.kaldiroglu.Composite.Bom.Solution</c>), and trimmed
/// to what the Iterator homework needs. The pattern folders in this repository do not
/// reference each other, so the types are copied rather than shared. The roll-ups (cost,
/// weight, part count), the caches and the parent links are left out.
/// </para>
/// </remarks>
public sealed record BomLine
{
    /// <summary>Creates a line.</summary>
    /// <param name="component">The child component.</param>
    /// <param name="quantity">How many are required, always at least one.</param>
    public BomLine(BomComponent component, int quantity)
    {
        ArgumentNullException.ThrowIfNull(component);
        if (quantity < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantity), quantity, "quantity must be at least 1");
        }

        Component = component;
        Quantity = quantity;
    }

    /// <summary>The child component.</summary>
    public BomComponent Component { get; }

    /// <summary>How many of the child the parent assembly requires.</summary>
    public int Quantity { get; }
}
