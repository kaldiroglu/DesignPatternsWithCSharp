namespace dev.kaldiroglu.Visitor.Checkout.Solution;

/// <summary>
/// The <b>Element</b>: an item accepts a visitor and calls it back.
/// <para>
/// That call back is the point. Inside <c>Book.Accept</c>, <c>this</c> has the static type
/// <c>Book</c>, so <c>visitor.Visit(this)</c> chooses <c>Visit(Book)</c> at compile time.
/// The first call chooses by the item's run-time type, the second by the visitor's: GoF call
/// this double dispatch.
/// </para>
/// </summary>
public interface IItem
{
    string Name { get; }

    int Price { get; }

    R Accept<R>(IItemVisitor<R> visitor);
}
