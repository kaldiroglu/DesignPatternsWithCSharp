using System.Globalization;

namespace dev.kaldiroglu.Visitor.Checkout.Solution;

/// <summary>A <b>ConcreteVisitor</b> that returns text: the line printed on the receipt.</summary>
public sealed class ReceiptLineVisitor : IItemVisitor<string>
{
    public string Visit(Book book) =>
        string.Create(CultureInfo.InvariantCulture, $"Book        {book.Name} {book.Price}");

    public string Visit(Food food) =>
        string.Create(CultureInfo.InvariantCulture, $"Food        {food.Name} {food.Price}");

    public string Visit(Electronics electronics) =>
        string.Create(CultureInfo.InvariantCulture, $"Electronics {electronics.Name} {electronics.Price}");

    public string Visit(GiftCard giftCard) =>
        string.Create(CultureInfo.InvariantCulture, $"Gift card   {giftCard.Name} {giftCard.Price} (sent by e-mail)");
}
