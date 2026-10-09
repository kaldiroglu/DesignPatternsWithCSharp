namespace dev.kaldiroglu.Visitor.Checkout.Problem;

public sealed record Electronics(string Name, int Price) : IItem;
