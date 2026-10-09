namespace dev.kaldiroglu.Visitor.Checkout.Solution;

/// <summary>
/// The <b>Visitor</b>: one method for each kind of item.
/// <para>
/// When the catalog team adds <see cref="GiftCard"/>, they add <c>Visit(GiftCard)</c> here,
/// and every visitor that does not handle gift cards stops compiling. A missing case is a
/// compile error, not a wrong total.
/// </para>
/// </summary>
/// <typeparam name="R">what the operation returns for one item</typeparam>
public interface IItemVisitor<R>
{
    R Visit(Book book);

    R Visit(Food food);

    R Visit(Electronics electronics);

    R Visit(GiftCard giftCard);
}
