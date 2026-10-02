namespace dev.kaldiroglu.Strategy.Pricing.Domain;

/// <summary>
/// What the customer is holding at the till.
/// <para>
/// A basket knows what is in it and what that costs at shelf price. It knows nothing about
/// campaigns — which is the point, and the thing every naive design in <c>Problem</c>
/// eventually breaks.
/// </para>
/// </summary>
public sealed record Basket(IReadOnlyList<Line> Lines, Customer Customer)
{
    public IReadOnlyList<Line> Lines { get; } = [.. Lines];

    public static Basket Of(Customer customer, params Line[] lines) => new(lines, customer);

    /// <summary>The shelf price of everything in the basket, before any campaign.</summary>
    public Money ListTotal
    {
        get
        {
            var total = Money.Zero;
            foreach (var line in Lines)
            {
                total = total.Plus(line.ListTotal);
            }
            return total;
        }
    }

    /// <summary>How many items are in the basket, counting quantities.</summary>
    public int ItemCount => Lines.Sum(line => line.Quantity);

    /// <summary>The lines in one category, cheapest first — what a "buy two, get one" rule needs.</summary>
    public IReadOnlyList<Line> InCategory(string category)
    {
        var found = new List<Line>();
        foreach (var line in Lines)
        {
            if (line.Category == category)
            {
                found.Add(line);
            }
        }
        // OrderBy is stable, as Java's List.sort is; List<T>.Sort is not.
        return [.. found.OrderBy(line => line.UnitPrice)];
    }
}
