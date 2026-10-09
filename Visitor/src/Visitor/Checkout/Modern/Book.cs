namespace dev.kaldiroglu.Visitor.Checkout.Modern;

public sealed record Book(string Name, int Price) : IItem;
