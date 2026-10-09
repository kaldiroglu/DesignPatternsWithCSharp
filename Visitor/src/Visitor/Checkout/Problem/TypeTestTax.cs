namespace dev.kaldiroglu.Visitor.Checkout.Problem;

/// <summary>
/// Stage three: one class for the tax, which tests the type of each item.
/// <para>
/// Correct for every item the shop sold when it was written. The last branch charges the
/// standard rate, so an item kind added later is taxed without any error — a gift card is
/// charged 20 percent, and nothing tells the developer that a branch is missing.
/// </para>
/// </summary>
public sealed class TypeTestTax
{
    public int TaxOf(IItem item)
    {
        if (item is Book book)
        {
            return book.Price * Rates.BookTax / 100;
        }
        else if (item is Food food)
        {
            return food.Price * Rates.FoodTax / 100;
        }
        else if (item is Electronics electronics)
        {
            return electronics.Price * Rates.ElectronicsTax / 100;
        }
        else
        {
            return item.Price * Rates.StandardTax / 100;
        }
    }

    public int Total(IReadOnlyList<IItem> cart) => cart.Sum(TaxOf);
}
