namespace dev.kaldiroglu.ChainOfResponsibility.Hw.Middleware;

/// <summary>Three links: one that logs and passes on, one that may stop the request, one that adds a header.</summary>
public static class Links
{
    /// <summary>Writes every path to the log, then passes the request on.</summary>
    public static Middleware Logging(IList<string> log) =>
        (request, next) =>
        {
            log.Add(request.User + " " + request.Path);
            return next(request);
        };

    /// <summary>Answers 403 itself for an admin page when the user is not an admin.</summary>
    public static Middleware AdminOnly() =>
        (request, next) =>
        {
            if (request.Path.StartsWith("/admin", StringComparison.Ordinal) && request.User != "admin")
            {
                return "403 forbidden";
            }
            return next(request);
        };

    /// <summary>Lets the rest of the chain answer, then adds a header to the answer.</summary>
    public static Middleware PoweredBy() =>
        (request, next) => next(request) + " [powered by the chain]";
}
