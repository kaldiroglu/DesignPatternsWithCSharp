namespace dev.kaldiroglu.Strategy.Pricing.Domain;

/// <summary>
/// One line on a basket: a product, its unit price, and how many of it.
/// </summary>
/// <param name="Sku">what the product is called on the shelf edge</param>
/// <param name="Category">what the campaign rules group it by</param>
/// <param name="UnitPrice">the shelf price of one, before any campaign</param>
/// <param name="Quantity">how many of them</param>
public sealed record Line(string Sku, string Category, Money UnitPrice, int Quantity)
{
    public int Quantity { get; } = Quantity >= 1
        ? Quantity
        : throw new ArgumentException("a line needs at least one of something", nameof(Quantity));

    /// <summary>What this line costs at shelf price, before any campaign touches it.</summary>
    public Money ListTotal => UnitPrice.Times(Quantity);
}
