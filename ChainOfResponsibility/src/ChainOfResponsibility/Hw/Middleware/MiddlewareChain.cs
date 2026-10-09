namespace dev.kaldiroglu.ChainOfResponsibility.Hw.Middleware;

/// <summary>
/// Builds a middleware chain. In the Java this is the static method <c>Middleware.chain</c>;
/// a delegate type cannot hold a method, so it lives in this class.
/// </summary>
public static class MiddlewareChain
{
    /// <summary>Builds the chain: the first middleware in the list runs first.</summary>
    public static Endpoint Chain(IReadOnlyList<Middleware> links, Endpoint end)
    {
        Endpoint next = end;
        for (int i = links.Count - 1; i >= 0; i--)
        {
            Middleware link = links[i];
            Endpoint rest = next;
            next = request => link(request, rest);
        }
        return next;
    }
}
