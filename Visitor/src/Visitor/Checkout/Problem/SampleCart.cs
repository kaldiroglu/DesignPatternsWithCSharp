namespace dev.kaldiroglu.Visitor.Checkout.Problem;

/// <summary>
/// The cart that stages two and three run on: a book for 40, food for 100 and electronics
/// for 500. Each call returns a new list, so a caller can add a gift card to it.
/// </summary>
internal static class SampleCart
{
    internal static List<IItem> ThreeItems() =>
        [new Book("Novel", 40), new Food("Coffee", 100), new Electronics("Headphones", 500)];
}
