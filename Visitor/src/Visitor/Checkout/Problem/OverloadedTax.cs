namespace dev.kaldiroglu.Visitor.Checkout.Problem;

/// <summary>
/// Stage two: one class for the tax, with one method per kind of item.
/// <para>
/// The items stay clean, and the tax rules are in one place. But <see cref="Total"/> holds
/// each item as an <c>IItem</c>. C# chooses between overloads at compile time, from the
/// static type of the argument, as Java does, so <c>TaxOf(item)</c> always calls
/// <c>TaxOf(IItem)</c>. Without that method the loop does not compile; with it, every item is
/// taxed at the standard rate.
/// </para>
/// </summary>
public sealed class OverloadedTax
{
    public int TaxOf(Book book) => book.Price * Rates.BookTax / 100;

    public int TaxOf(Food food) => food.Price * Rates.FoodTax / 100;

    public int TaxOf(Electronics electronics) => electronics.Price * Rates.ElectronicsTax / 100;

    public int TaxOf(IItem item) => item.Price * Rates.StandardTax / 100;

    public int Total(IReadOnlyList<IItem> cart)
    {
        int total = 0;
        foreach (IItem item in cart)
        {
            total += TaxOf(item);       // always TaxOf(IItem)
        }
        return total;
    }
}
