namespace dev.kaldiroglu.Visitor.Checkout.Solution;

/// <summary>A <b>ConcreteVisitor</b>: the whole shipping rule, in one class.</summary>
public sealed class ShippingVisitor : IItemVisitor<int>
{
    public int Visit(Book book) => Rates.BookShipping;

    public int Visit(Food food) => Rates.FoodShipping;

    public int Visit(Electronics electronics) => Rates.ElectronicsShipping;

    public int Visit(GiftCard giftCard) => 0;      // sent by e-mail
}
