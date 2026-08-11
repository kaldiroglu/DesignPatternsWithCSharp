using dev.kaldiroglu.Decorator.Middleware.Domain;

namespace dev.kaldiroglu.Decorator.Middleware.Solution.Classic;

/// <summary>
/// The <b>Decorator</b>. Eighteen lines, and the entire pattern is in them.
/// <para>
/// It <i>is</i> an <see cref="IPriceFeed"/>, so a decorated feed goes anywhere a feed
/// goes — including inside another decorator, which is what makes chains possible. And it
/// <i>has</i> one, handed in by the constructor: that parameter is the only place a chain
/// is ever joined.
/// </para>
/// </summary>
public abstract class PriceFeedDecorator : IPriceFeed
{
    private readonly IPriceFeed _inner;

    protected PriceFeedDecorator(IPriceFeed inner) =>
        _inner = inner ?? throw new ArgumentNullException(
            nameof(inner), "a decorator must decorate something");

    /// <summary>
    /// The wrapped feed. Declared <see cref="IPriceFeed"/>, so it is any feed: the
    /// supplier, another decorator, or a whole chain.
    /// </summary>
    protected IPriceFeed Inner() => _inner;

    public virtual Quote QuoteFor(string sku) => _inner.QuoteFor(sku);
}
