namespace dev.kaldiroglu.Visitor.Checkout.Problem;

public sealed record Book(string Name, int Price) : IItem;
