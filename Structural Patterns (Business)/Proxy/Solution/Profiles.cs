using System.Collections.Concurrent;

namespace DevKaldiroglu.DP.Structural.Proxy.Solution;

public sealed record CustomerProfile(string CustomerId, string Name, string Region);
public sealed record Caller(string UserId, string Role, string Region);

public interface ICustomerProfileService
{
    CustomerProfile GetProfile(Caller caller, string customerId);
}

public class RemoteCustomerProfileService : ICustomerProfileService
{
    private int _backendCalls;

    public CustomerProfile GetProfile(Caller caller, string customerId)
    {
        Interlocked.Increment(ref _backendCalls);
        return new CustomerProfile(customerId, "Customer-" + customerId,
            customerId.StartsWith("EU") ? "EU" : "US");
    }

    public int BackendCalls => _backendCalls;
}

public interface IAccessPolicy
{
    bool CanRead(Caller caller, string customerId);
}

public class RegionalAccessPolicy : IAccessPolicy
{
    public bool CanRead(Caller caller, string customerId)
    {
        if (caller.Role == "admin") return true;
        var customerRegion = customerId.StartsWith("EU") ? "EU" : "US";
        return caller.Region == customerRegion;
    }
}

/// <summary>Protection proxy. Sits OUTSIDE the cache so unauthorized requests
/// are never served from a cached entry — every caller is re-checked.</summary>
public class AuthorizationProxy : ICustomerProfileService
{
    private readonly ICustomerProfileService _inner;
    private readonly IAccessPolicy _policy;

    public AuthorizationProxy(ICustomerProfileService inner, IAccessPolicy policy)
    {
        _inner = inner; _policy = policy;
    }

    public CustomerProfile GetProfile(Caller caller, string customerId)
    {
        if (!_policy.CanRead(caller, customerId))
            throw new UnauthorizedAccessException($"denied: {caller.UserId} -> {customerId}");
        return _inner.GetProfile(caller, customerId);
    }
}

/// <summary>Caching proxy. Keyed only on customerId; access decisions happen upstream.</summary>
public class CachingProxy : ICustomerProfileService
{
    private record Entry(CustomerProfile Value, DateTimeOffset Expires);

    private readonly ICustomerProfileService _inner;
    private readonly TimeSpan _ttl;
    private readonly ConcurrentDictionary<string, Entry> _cache = new();

    public CachingProxy(ICustomerProfileService inner, TimeSpan ttl)
    {
        _inner = inner; _ttl = ttl;
    }

    public CustomerProfile GetProfile(Caller caller, string customerId)
    {
        var now = DateTimeOffset.UtcNow;
        if (_cache.TryGetValue(customerId, out var hit) && now < hit.Expires)
            return hit.Value;

        var fresh = _inner.GetProfile(caller, customerId);
        _cache[customerId] = new Entry(fresh, now + _ttl);
        return fresh;
    }
}

public static class SolutionDemo
{
    public static void Run()
    {
        var backend = new RemoteCustomerProfileService();

        ICustomerProfileService service =
            new AuthorizationProxy(
                new CachingProxy(backend, TimeSpan.FromMinutes(5)),
                new RegionalAccessPolicy());

        var usAgent = new Caller("agent-1", "support", "US");
        var euAgent = new Caller("agent-2", "support", "EU");
        var admin   = new Caller("root",    "admin",   "US");

        Console.WriteLine(service.GetProfile(usAgent, "US-1001"));
        Console.WriteLine(service.GetProfile(usAgent, "US-1001")); // cached
        Console.WriteLine(service.GetProfile(admin,   "US-1001")); // also cached, admin allowed

        try { service.GetProfile(usAgent, "EU-2002"); }
        catch (UnauthorizedAccessException e) { Console.WriteLine("denied: " + e.Message); }

        Console.WriteLine(service.GetProfile(euAgent, "EU-2002")); // backend hit
        Console.WriteLine(service.GetProfile(admin,   "EU-2002")); // cache hit

        Console.WriteLine($"backend calls: {backend.BackendCalls}");
    }
}
