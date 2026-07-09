namespace DevKaldiroglu.DP.Structural.Decorator.Problem;

public class HttpClient
{
    private int _callCount;
    private bool _loggingEnabled;
    private bool _retryEnabled;
    private int _maxRetries;
    private bool _cachingEnabled;
    private readonly Dictionary<string, string> _cache = new();
    private string? _authToken;

    public HttpClient WithLogging(bool v)        { _loggingEnabled = v; return this; }
    public HttpClient WithRetry(bool v, int n)   { _retryEnabled = v; _maxRetries = n; return this; }
    public HttpClient WithCache(bool v)          { _cachingEnabled = v; return this; }
    public HttpClient WithAuth(string token)     { _authToken = token; return this; }

    public string Send(string method, string url)
    {
        if (_cachingEnabled && method == "GET" && _cache.TryGetValue(url, out var cached))
        {
            if (_loggingEnabled) Console.WriteLine($"LOG cache-hit {url}");
            return cached;
        }

        var attempts = _retryEnabled ? _maxRetries + 1 : 1;
        Exception? last = null;
        for (int i = 0; i < attempts; i++)
        {
            try
            {
                if (_loggingEnabled)
                    Console.WriteLine($"LOG try {i + 1}/{attempts} {method} {url}");
                var response = DoSend(method, url);
                if (_cachingEnabled && method == "GET") _cache[url] = response;
                return response;
            }
            catch (Exception e) { last = e; }
        }
        throw last!;
    }

    private string DoSend(string method, string url)
    {
        var n = Interlocked.Increment(ref _callCount);
        if (n % 3 == 0 && url.EndsWith("/users/42")) throw new Exception("502 bad gateway");
        var auth = _authToken ?? "anon";
        return $"{{\"auth\":\"{auth}\",\"url\":\"{url}\"}}";
    }
}

public static class ProblemDemo
{
    public static void Run()
    {
        var client = new HttpClient()
            .WithLogging(true)
            .WithRetry(true, 2)
            .WithCache(true)
            .WithAuth("token-xyz");

        Console.WriteLine(client.Send("GET", "/users/42"));
        Console.WriteLine(client.Send("GET", "/users/42")); // cached
    }
}
