namespace DevKaldiroglu.DP.Structural.Decorator.Solution;

public sealed class HttpRequest
{
    public string Method { get; }
    public string Url { get; }
    public IReadOnlyDictionary<string, string> Headers { get; }

    public HttpRequest(string method, string url)
        : this(method, url, new Dictionary<string, string>()) { }

    private HttpRequest(string method, string url, IReadOnlyDictionary<string, string> headers)
    {
        Method = method; Url = url; Headers = headers;
    }

    public HttpRequest WithHeader(string name, string value)
    {
        var next = new Dictionary<string, string>(Headers) { [name] = value };
        return new HttpRequest(Method, Url, next);
    }
}

public sealed record HttpResponse(int Status, string Body);

public interface IHttpClient
{
    HttpResponse Send(HttpRequest request);
}

public class RealHttpClient : IHttpClient
{
    private int _callCount;

    public HttpResponse Send(HttpRequest req)
    {
        var n = Interlocked.Increment(ref _callCount);
        if (n % 3 == 0 && req.Url.EndsWith("/users/42"))
            return new HttpResponse(502, "bad gateway");
        var auth = req.Headers.GetValueOrDefault("Authorization", "anon");
        return new HttpResponse(200, $"{{\"auth\":\"{auth}\",\"url\":\"{req.Url}\"}}");
    }
}

public abstract class HttpClientDecorator : IHttpClient
{
    protected readonly IHttpClient Delegate;
    protected HttpClientDecorator(IHttpClient inner) => Delegate = inner;
    public virtual HttpResponse Send(HttpRequest request) => Delegate.Send(request);
}

public class AuthDecorator : HttpClientDecorator
{
    private readonly Func<string> _tokenSupplier;
    public AuthDecorator(IHttpClient inner, Func<string> tokenSupplier) : base(inner) => _tokenSupplier = tokenSupplier;

    public override HttpResponse Send(HttpRequest request) =>
        Delegate.Send(request.WithHeader("Authorization", "Bearer " + _tokenSupplier()));
}

public class RetryDecorator : HttpClientDecorator
{
    private readonly int _maxAttempts;
    public RetryDecorator(IHttpClient inner, int maxAttempts) : base(inner)
    {
        if (maxAttempts < 1) throw new ArgumentOutOfRangeException(nameof(maxAttempts));
        _maxAttempts = maxAttempts;
    }

    public override HttpResponse Send(HttpRequest request)
    {
        HttpResponse? last = null;
        for (int i = 0; i < _maxAttempts; i++)
        {
            last = Delegate.Send(request);
            if (last.Status < 500) return last;
        }
        return last!;
    }
}

public class LoggingDecorator : HttpClientDecorator
{
    public LoggingDecorator(IHttpClient inner) : base(inner) { }

    public override HttpResponse Send(HttpRequest request)
    {
        var start = System.Diagnostics.Stopwatch.GetTimestamp();
        var response = Delegate.Send(request);
        var elapsedMicros = (long)((System.Diagnostics.Stopwatch.GetTimestamp() - start) * 1_000_000.0
                                   / System.Diagnostics.Stopwatch.Frequency);
        Console.WriteLine($"LOG {request.Method} {request.Url} -> {response.Status} ({elapsedMicros}us)");
        return response;
    }
}

public class CachingDecorator : HttpClientDecorator
{
    private record Entry(HttpResponse Response, DateTimeOffset Expires);

    private readonly Dictionary<string, Entry> _cache = new();
    private readonly TimeSpan _ttl;

    public CachingDecorator(IHttpClient inner, TimeSpan ttl) : base(inner) => _ttl = ttl;

    public override HttpResponse Send(HttpRequest request)
    {
        if (!string.Equals(request.Method, "GET", StringComparison.OrdinalIgnoreCase))
            return Delegate.Send(request);

        var key = CacheKey(request);
        var now = DateTimeOffset.UtcNow;
        if (_cache.TryGetValue(key, out var hit) && now < hit.Expires) return hit.Response;

        var response = Delegate.Send(request);
        if (response.Status is >= 200 and < 300)
            _cache[key] = new Entry(response, now + _ttl);
        return response;
    }

    private static string CacheKey(HttpRequest r) =>
        $"{r.Url}|{r.Headers.GetValueOrDefault("Authorization", "")}";
}

public static class SolutionDemo
{
    public static void Run()
    {
        IHttpClient internalClient =
            new LoggingDecorator(
                new CachingDecorator(
                    new RetryDecorator(
                        new AuthDecorator(new RealHttpClient(), () => "token-xyz"),
                        3),
                    TimeSpan.FromSeconds(30)));

        IHttpClient probe = new RealHttpClient();

        Console.WriteLine(internalClient.Send(new HttpRequest("GET", "/users/42")));
        Console.WriteLine(internalClient.Send(new HttpRequest("GET", "/users/42"))); // cache hit
        Console.WriteLine(probe.Send(new HttpRequest("GET", "/healthz")));
    }
}
