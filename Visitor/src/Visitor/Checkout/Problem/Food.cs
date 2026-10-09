namespace dev.kaldiroglu.Visitor.Checkout.Problem;

public sealed record Food(string Name, int Price) : IItem;
