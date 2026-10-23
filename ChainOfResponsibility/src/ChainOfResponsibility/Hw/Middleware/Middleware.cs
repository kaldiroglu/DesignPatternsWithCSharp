namespace dev.kaldiroglu.ChainOfResponsibility.Hw.Middleware;

/// <summary>
/// Homework 2: a chain where every link may act, and most links pass the request on.
/// <para>
/// GoF's chain stops at the first link that handles the request. A middleware chain is the
/// other form: each link does its own work — logging, checking — and then calls the rest
/// of the chain, unless it decides to answer itself. The link is a lambda that is given
/// the request and the rest of the chain.
/// </para>
/// <para>
/// A delegate cannot hold a method, so the method that builds a chain is
/// <see cref="MiddlewareChain.Chain"/>.
/// </para>
/// </summary>
public delegate string Middleware(HttpRequest request, Endpoint next);
