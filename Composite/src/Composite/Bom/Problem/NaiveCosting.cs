using dev.kaldiroglu.Composite.Bom.Domain;

namespace dev.kaldiroglu.Composite.Bom.Problem;

/// <summary>
/// The first client of the naive design: the quotation screen's costing routine.
/// </summary>
/// <remarks>
/// <para>
/// Because <see cref="Part"/> and <see cref="Assembly"/> share no base type, the
/// operation cannot be a method on the thing being costed. It becomes a static
/// function that takes <see cref="object"/> — and the moment a parameter is
/// <c>object</c>, the type system has stopped helping.
/// </para>
/// <para>
/// Read the two methods below and notice that they are the <em>same walk, written
/// twice</em>. Then read <see cref="NaiveShipping"/> and notice it is the same walk
/// a third time. That is the duplication Composite removes: in <c>..Bom.Solution</c>
/// the walk appears once, inside the composite itself, and every client is a single
/// method call.
/// </para>
/// </remarks>
public static class NaiveCosting
{
    /// <summary>The total cost of an item, whatever kind of item it is.</summary>
    /// <param name="node">A <see cref="Part"/> or an <see cref="Assembly"/>.</param>
    /// <exception cref="ArgumentException">
    /// For anything else — including <see cref="Service"/>, which was added to the
    /// domain later.
    /// </exception>
    public static Money TotalCost(object node)
    {
        if (node is Part part)
        {
            return part.UnitCost;
        }

        if (node is Assembly assembly)
        {
            var total = assembly.AssemblyCost;
            foreach (var p in assembly.Parts)               // first collection
            {
                total = total.Plus(p.UnitCost);
            }

            foreach (var sub in assembly.SubAssemblies)      // second collection
            {
                total = total.Plus(TotalCost(sub));
            }

            return total;
        }

        // Every new kind of item lands here until somebody edits this method.
        throw new ArgumentException($"NaiveCosting cannot cost a {node.GetType().Name}");
    }

    /// <summary>The number of purchasable parts in an item.</summary>
    /// <remarks>
    /// The same recursion again, with two words changed. A reader has to compare it
    /// line by line with <see cref="TotalCost"/> to be sure it visits the same nodes
    /// — and it is exactly this kind of near-duplicate that quietly drifts out of
    /// step when the structure changes.
    /// </remarks>
    public static int PartCount(object node)
    {
        if (node is Part)
        {
            return 1;
        }

        if (node is Assembly assembly)
        {
            var total = assembly.Parts.Count;
            foreach (var sub in assembly.SubAssemblies)
            {
                total += PartCount(sub);
            }

            return total;
        }

        throw new ArgumentException($"NaiveCosting cannot count a {node.GetType().Name}");
    }
}
