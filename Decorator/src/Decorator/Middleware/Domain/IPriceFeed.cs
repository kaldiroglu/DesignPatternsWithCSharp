namespace dev.kaldiroglu.Decorator.Middleware.Domain;

/// <summary>
/// The <b>Component</b>: one method, no state.
/// <para>
/// GoF implementation issue 3 (p. 180) says to keep the Component lightweight, and there
/// is a second payoff for doing so here — with a single method this interface can be
/// satisfied by a lambda, which is what the Functional variation depends on.
/// </para>
/// </summary>
public interface IPriceFeed
{
    Quote QuoteFor(string sku);
}
