namespace dev.kaldiroglu.ChainOfResponsibility.Hw.Middleware;

/// <summary>
/// The end of the line: something that turns a request into a response.
/// <para>
/// It is a delegate, so the next step is called as <c>next(request)</c>.
/// </para>
/// </summary>
public delegate string Endpoint(HttpRequest request);
