namespace dev.kaldiroglu.Visitor.Checkout.Modern;

public sealed record Food(string Name, int Price) : IItem;
