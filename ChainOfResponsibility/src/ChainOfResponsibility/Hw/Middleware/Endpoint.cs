namespace dev.kaldiroglu.ChainOfResponsibility.Hw.Middleware;

/// <summary>
/// The end of the line: something that turns a request into a response.
/// <para>
/// The Java is a functional interface with the method <c>serve</c>. In C# it is a delegate,
/// so <c>next.serve(request)</c> is written <c>next(request)</c>.
/// </para>
/// </summary>
public delegate string Endpoint(HttpRequest request);
