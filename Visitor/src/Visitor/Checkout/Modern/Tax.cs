namespace dev.kaldiroglu.Visitor.Checkout.Modern;

/// <summary>
/// The tax rule as one switch.
/// <para>
/// In Java the switch has no <c>default</c>: if a fifth item kind is permitted, the switch
/// stops compiling until it has a case for it. In C# the last arm, <c>_</c>, is required —
/// without it the compiler gives warning CS8509 ("The switch expression does not handle all
/// possible values of its input type"), because <see cref="IItem"/> is open to other classes. With it, a fifth
/// item kind compiles, and this method throws when it meets one.
/// </para>
/// </summary>
public static class Tax
{
    public static int Of(IItem item) => item switch
    {
        Book book => book.Price * 5 / 100,
        Food food => food.Price * 1 / 100,
        Electronics electronics => electronics.Price * 20 / 100,
        GiftCard => 0,
        _ => throw new InvalidOperationException("No tax rule for " + item.GetType().Name)
    };

    public static int Total(IReadOnlyList<IItem> cart) => cart.Sum(Of);
}
