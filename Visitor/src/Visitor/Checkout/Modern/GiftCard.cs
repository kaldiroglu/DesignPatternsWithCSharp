namespace dev.kaldiroglu.Visitor.Checkout.Modern;

public sealed record GiftCard(string Name, int Price) : IItem;
