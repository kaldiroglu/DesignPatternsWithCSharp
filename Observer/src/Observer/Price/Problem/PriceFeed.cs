namespace dev.kaldiroglu.Observer.Price.Problem;

/// <summary>
/// Stages two and three: <b>a feed that knows nobody.</b>
/// <para>
/// It only holds the price. Readers ask for it when they want it — they poll. It also
/// counts how often it is asked, to show how much of the polling finds nothing new.
/// </para>
/// </summary>
/// <remarks>
/// <see cref="Price"/> stays a method, not a property, because every call counts as a read.
/// A debugger that shows property values would read the price, and change the count.
/// </remarks>
public sealed class PriceFeed(int price)
{
    private int price = price;
    private int reads;

    public void SetPrice(int price)
    {
        this.price = price;
    }

    public int Price()
    {
        reads++;
        return price;
    }

    public int Reads => reads;
}
