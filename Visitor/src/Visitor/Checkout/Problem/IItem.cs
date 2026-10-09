namespace dev.kaldiroglu.Visitor.Checkout.Problem;

/// <summary>
/// An item the shop sells, as the catalog team writes it: data only.
/// <para>
/// The interface is not sealed. The catalog team adds new kinds of item when the shop starts
/// selling them, and checkout code is not told.
/// </para>
/// </summary>
public interface IItem
{
    string Name { get; }

    int Price { get; }
}
