namespace dev.kaldiroglu.Observer.Price.Solution;

/// <summary>The event: what changed, from what, to what. Sent to every listener.</summary>
/// <remarks>
/// <see cref="PriceFeedWithEvent"/> also sends it as the event data of a C#
/// <c>event</c>. <see cref="EventHandler{TEventArgs}"/> does not require the type to extend
/// <see cref="EventArgs"/>, so a plain record works.
/// </remarks>
public sealed record PriceChange(string Symbol, int OldPrice, int NewPrice);
