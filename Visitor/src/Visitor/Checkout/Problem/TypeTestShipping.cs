namespace dev.kaldiroglu.Visitor.Checkout.Problem;

/// <summary>Stage three again, for shipping: the same chain of type tests, with the same last branch.</summary>
public sealed class TypeTestShipping
{
    public int CostOf(IItem item)
    {
        if (item is Book)
        {
            return Rates.BookShipping;
        }
        else if (item is Food)
        {
            return Rates.FoodShipping;
        }
        else if (item is Electronics)
        {
            return Rates.ElectronicsShipping;
        }
        else
        {
            return Rates.StandardShipping;
        }
    }

    public int Total(IReadOnlyList<IItem> cart) => cart.Sum(CostOf);
}
