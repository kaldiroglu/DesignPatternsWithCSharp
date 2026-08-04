using dev.kaldiroglu.Composite.Bom.Domain;

namespace dev.kaldiroglu.Composite.Bom.Solution;

/// <summary>
/// One line of a bill of materials: a component and how many of it the parent
/// assembly requires.
/// </summary>
/// <remarks>
/// Putting the quantity on the <em>edge</em> rather than the node is what lets a
/// single <see cref="Assembly"/> instance be shared by several parents — a
/// bicycle needs two identical wheels, not two wheel objects. GoF discusses this
/// under "Sharing components" (p. 167), and points at Flyweight (p. 195) as the
/// pattern for pushing it further.
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

    /// <summary>The cost contributed by this line: the child's total, times the quantity.</summary>
    public Money ExtendedCost() => Component.TotalCost().Times(Quantity);

    /// <summary>The mass contributed by this line, in grams.</summary>
    public int ExtendedWeightGrams() => Component.TotalWeightGrams() * Quantity;

    /// <summary>The number of purchasable parts contributed by this line.</summary>
    public int ExtendedPartCount() => Component.PartCount() * Quantity;
}
