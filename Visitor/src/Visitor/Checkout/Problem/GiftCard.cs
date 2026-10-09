namespace dev.kaldiroglu.Visitor.Checkout.Problem;

/// <summary>
/// Added by the catalog team after checkout was written. A gift card carries no tax — the
/// tax is charged when it is spent — and it is sent by e-mail, so it is not shipped.
/// </summary>
public sealed record GiftCard(string Name, int Price) : IItem;
