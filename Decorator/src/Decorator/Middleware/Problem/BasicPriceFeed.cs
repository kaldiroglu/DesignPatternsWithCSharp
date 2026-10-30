using dev.kaldiroglu.Decorator.Middleware.Domain;

namespace dev.kaldiroglu.Decorator.Middleware.Problem;

/// <summary>
/// Naive design 3, the root. It has no field: it derives from the supplier, so
/// <c>base.QuoteFor</c> is the supplier, fixed at compile time. A <c>sealed</c> class such as
/// <see cref="VendorPriceFeed"/> cannot take its place.
/// </summary>
public class BasicPriceFeed(ManualClock clock) : SimulatedRemotePriceFeed(clock, TimeSpan.FromMilliseconds(200));
