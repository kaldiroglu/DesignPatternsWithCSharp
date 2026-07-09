namespace DevKaldiroglu.DP.Structural.Proxy.Problem;

public sealed record CustomerProfile(string CustomerId, string Name, string Region);

public class CustomerProfileService
{
    private int _backendCalls;

    public CustomerProfile GetProfile(string customerId)
    {
        Interlocked.Increment(ref _backendCalls);
        return new CustomerProfile(customerId, "Customer-" + customerId,
            customerId.StartsWith("EU") ? "EU" : "US");
    }

    public int BackendCalls => _backendCalls;
}

public class SupportConsole
{
    private readonly CustomerProfileService _service;
    private readonly Dictionary<string, CustomerProfile> _localCache = new();

    public SupportConsole(CustomerProfileService service) => _service = service;

    public CustomerProfile Lookup(string agentRegion, string customerId)
    {
        if (_localCache.TryGetValue(customerId, out var cached)) return cached;

        var euAgent = agentRegion == "EU";
        var euCustomer = customerId.StartsWith("EU");
        if (euAgent != euCustomer) throw new UnauthorizedAccessException("region mismatch");

        var p = _service.GetProfile(customerId);
        _localCache[customerId] = p;
        return p;
    }
}

public static class ProblemDemo
{
    public static void Run()
    {
        var backend = new CustomerProfileService();
        var console = new SupportConsole(backend);

        Console.WriteLine(console.Lookup("US", "US-1001"));
        Console.WriteLine(console.Lookup("US", "US-1001")); // cached, but only here
        try { console.Lookup("US", "EU-2002"); }
        catch (UnauthorizedAccessException e) { Console.WriteLine("denied: " + e.Message); }
        Console.WriteLine($"backend calls: {backend.BackendCalls}");
    }
}
