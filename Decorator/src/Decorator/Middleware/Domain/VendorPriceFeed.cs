namespace dev.kaldiroglu.Decorator.Middleware.Domain;

/// <summary>
/// A price feed that arrived in a vendor's assembly, and is <c>sealed</c>.
/// <para>
/// This class exists to settle an argument. Adding behavior by subclassing is not merely
/// awkward here — it is <i>impossible</i>, and the compiler says so. Decoration needs
/// nothing from the class it wraps except that it implement the interface, so every
/// decorator in the Solution namespace works with this feed unchanged.
/// </para>
/// <para>
/// Vendors really do this, and so does the framework: <c>string</c>, most records and a
/// great many BCL types are sealed for good reasons of their own.
/// </para>
/// </summary>
public sealed class VendorPriceFeed : IPriceFeed
{
    public Quote QuoteFor(string sku)
    {
        CallCount++;
        return Quote.Of(sku, "42.00"); // the vendor's flat rate
    }

    public int CallCount { get; private set; }
}
