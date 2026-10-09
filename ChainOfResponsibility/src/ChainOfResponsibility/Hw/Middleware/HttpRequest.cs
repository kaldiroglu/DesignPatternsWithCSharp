namespace dev.kaldiroglu.ChainOfResponsibility.Hw.Middleware;

/// <summary>A request to a web server: the path and the user who sent it.</summary>
public sealed record HttpRequest(string Path, string User);
