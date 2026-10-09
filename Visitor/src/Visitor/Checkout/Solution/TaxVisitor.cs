namespace dev.kaldiroglu.Visitor.Checkout.Solution;

/// <summary>A <b>ConcreteVisitor</b>: the whole tax rule, in one class.</summary>
public sealed class TaxVisitor : IItemVisitor<int>
{
    public int Visit(Book book) => book.Price * Rates.BookTax / 100;

    public int Visit(Food food) => food.Price * Rates.FoodTax / 100;

    public int Visit(Electronics electronics) => electronics.Price * Rates.ElectronicsTax / 100;

    public int Visit(GiftCard giftCard) => 0;      // taxed when it is spent
}
