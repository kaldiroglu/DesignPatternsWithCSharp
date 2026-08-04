using System.Text;

using dev.kaldiroglu.Composite.Bom.Domain;

namespace dev.kaldiroglu.Composite.Bom.Solution;

/// <summary>
/// Component role of the Composite pattern — one entry in a bill of materials.
/// </summary>
/// <remarks>
/// <para>
/// A bill of materials describes what a manufactured product is made of. A
/// bicycle contains wheels; a wheel contains a rim, spokes and a hub; a hub
/// contains an axle and bearings. The nesting has no fixed depth and differs from
/// product to product.
/// </para>
/// <para>
/// The whole point of this abstraction is that the four questions the business
/// actually asks — <em>what does it cost, what does it weigh, how many parts is
/// it, and show me the structure</em> — are answered the same way for a single
/// bearing and for a finished bicycle. Client code such as a quotation screen or
/// a shipping calculator therefore never branches on "part or assembly?".
/// </para>
/// <para>
/// <b>On the safety/transparency trade-off (GoF, p. 168):</b> unlike the book's
/// own examples, this one declares <c>Add</c> on <see cref="Assembly"/> only,
/// <em>not</em> here. A bill of materials is edited by a small amount of
/// engineering-change code that legitimately knows it is holding an assembly,
/// while the many read-only clients — costing, shipping, reporting — only ever
/// call the query operations below. Restricting the child operations to the
/// composite buys compile-time safety and costs those read-only clients nothing.
/// </para>
/// </remarks>
public abstract class BomComponent(string partNumber, string name)
{
    /// <summary>The catalog identifier, e.g. <c>"RIM-700C"</c>.</summary>
    public string PartNumber { get; } = partNumber;

    /// <summary>The human-readable name, e.g. <c>"700c Rim"</c>.</summary>
    public string Name { get; } = name;

    // --- The operations every client cares about, leaf and composite alike ---

    /// <summary>The total cost of this component including everything inside it.</summary>
    public abstract Money TotalCost();

    /// <summary>The total mass in grams, including everything inside it.</summary>
    public abstract int TotalWeightGrams();

    /// <summary>
    /// The number of individual purchasable parts inside this component,
    /// counting quantities. A part counts as one; an assembly counts the sum of
    /// its lines.
    /// </summary>
    public abstract int PartCount();

    /// <summary>
    /// The lines that make up this component — empty for a part.
    /// </summary>
    /// <remarks>
    /// A read-only view is safe to offer on a leaf, and it is what lets client
    /// code walk any subtree recursively without a type test.
    /// </remarks>
    public virtual IReadOnlyList<BomLine> Lines => Array.Empty<BomLine>();

    /// <summary>Whether this component is an assembly that can contain others.</summary>
    public virtual bool IsAssembly => false;

    /// <summary>
    /// Whether <paramref name="target"/> appears anywhere strictly below this
    /// component.
    /// </summary>
    /// <remarks>
    /// Used by <see cref="Assembly.Add(BomComponent, int)"/> to keep the product
    /// structure acyclic. A leaf answers <c>false</c> without any special casing,
    /// because it has no lines to search.
    /// </remarks>
    internal bool ContainsDeep(BomComponent target)
    {
        foreach (var line in Lines)
        {
            if (ReferenceEquals(line.Component, target) || line.Component.ContainsDeep(target))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>Renders this component and everything below it as an indented tree.</summary>
    public string ToTree()
    {
        var output = new StringBuilder();
        AppendTree(output, "", 1);
        return output.ToString();
    }

    /// <summary>
    /// Appends one line for this component and then recurses into its children.
    /// </summary>
    /// <param name="output">The buffer being built.</param>
    /// <param name="indent">The prefix for this level.</param>
    /// <param name="quantity">How many of this component the parent requires.</param>
    protected void AppendTree(StringBuilder output, string indent, int quantity)
    {
        output.Append(indent)
            .Append(quantity > 1 ? $"{quantity}x " : "")
            .Append(Name)
            .Append(" [").Append(PartNumber).Append(']')
            .Append("  cost ").Append(TotalCost().Times(quantity))
            .Append(", ").Append(TotalWeightGrams() * quantity).Append(" g")
            .AppendLine();

        foreach (var line in Lines)
        {
            line.Component.AppendTree(output, indent + "    ", line.Quantity);
        }
    }
}
