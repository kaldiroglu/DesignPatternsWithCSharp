namespace dev.kaldiroglu.Visitor.Checkout.Solution;

/// <summary>The <b>ObjectStructure</b>: the cart. It walks its items and sends each one the visitor.</summary>
public sealed class Checkout
{
    private readonly IReadOnlyList<IItem> cart;

    public Checkout(IReadOnlyList<IItem> cart)
    {
        this.cart = cart.ToList();
    }

    public int Total(IItemVisitor<int> visitor)
    {
        int total = 0;
        foreach (IItem item in cart)
        {
            total += item.Accept(visitor);
        }
        return total;
    }

    public IReadOnlyList<string> Lines(IItemVisitor<string> visitor) =>
        cart.Select(item => item.Accept(visitor)).ToList();
}
