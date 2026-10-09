namespace dev.kaldiroglu.Visitor.Checkout.Problem.Methods;

/// <summary>
/// Stage one: every operation is a method on the item.
/// <para>
/// Each item class knows its own tax, its own shipping cost and its own receipt line. It
/// works, and it is fast to write. But the tax rules are now in three classes, and the next
/// operation — customs codes, loyalty points — is an edit to every one of them. The item
/// classes belong to the catalog; the operations belong to checkout.
/// </para>
/// </summary>
public interface IItem
{
    string Name { get; }

    int Price { get; }

    int Tax();

    int ShippingCost();

    string ReceiptLine();
}
