using dev.kaldiroglu.Composite.Bom.Domain;

namespace dev.kaldiroglu.Composite.Bom.Problem;

/// <summary>
/// The second client of the naive design: the shipping calculator, written months
/// later by a different team.
/// </summary>
/// <remarks>
/// <para>
/// They could not reuse anything from <see cref="NaiveCosting"/>, because there is
/// nothing to reuse — the recursion is welded to the thing being computed. So they
/// wrote the walk again. It is now in this codebase three times, and every future
/// client will make it four.
/// </para>
/// <para>
/// The bug this invites is not hypothetical. If a fourth collection is ever added to
/// <see cref="Assembly"/>, or if someone forgets the <c>SubAssemblies</c> loop in one
/// of the three copies, the answer is silently wrong — no exception, no compiler
/// error, just a quotation that is too low.
/// </para>
/// </remarks>
public static class NaiveShipping
{
    private static readonly Money RatePerKilo = Money.Of(4.90m);

    /// <summary>
    /// The mass of an item in grams — the same walk as
    /// <see cref="NaiveCosting.TotalCost"/>, for the third time.
    /// </summary>
    public static int TotalWeightGrams(object node)
    {
        if (node is Part part)
        {
            return part.WeightGrams;
        }

        if (node is Assembly assembly)
        {
            var total = assembly.AssemblyWeightGrams;
            foreach (var p in assembly.Parts)
            {
                total += p.WeightGrams;
            }

            foreach (var sub in assembly.SubAssemblies)
            {
                total += TotalWeightGrams(sub);
            }

            return total;
        }

        throw new ArgumentException($"NaiveShipping cannot weigh a {node.GetType().Name}");
    }

    /// <summary>What it costs to ship an item, rounded up to the next whole kilo.</summary>
    public static Money Estimate(object node)
    {
        var kilos = Math.Max(1, (TotalWeightGrams(node) + 999) / 1000);
        return RatePerKilo.Times(kilos);
    }
}
