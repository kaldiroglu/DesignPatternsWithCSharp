using System.Globalization;

namespace dev.kaldiroglu.Visitor.Checkout.Problem.Methods;

/// <summary>Stage one: the item computes its own tax, shipping cost and receipt line.</summary>
public sealed record Food(string Name, int Price) : IItem
{
    public int Tax() => Price * 1 / 100;

    public int ShippingCost() => 15;

    public string ReceiptLine() => string.Create(CultureInfo.InvariantCulture, $"{Name} {Price} (tax {Tax()})");
}
