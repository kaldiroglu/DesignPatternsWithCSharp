using System.Globalization;

namespace dev.kaldiroglu.Visitor.Checkout.Problem.Methods;

/// <summary>Stage one: the item computes its own tax, shipping cost and receipt line.</summary>
public sealed record Electronics(string Name, int Price) : IItem
{
    public int Tax() => Price * 20 / 100;

    public int ShippingCost() => 25;

    public string ReceiptLine() => string.Create(CultureInfo.InvariantCulture, $"{Name} {Price} (tax {Tax()})");
}
