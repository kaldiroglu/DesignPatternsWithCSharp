# Proxy — Caching + Authorization Stack

`CustomerProfileService` is a slow, read-heavy service backed by a remote system. Two cross-cutting concerns sit on top of it:

- **Authorization** — callers can only read profiles they're entitled to.
- **Caching** — a profile rarely changes; repeated reads within a short TTL should not hit the backend.

## Without the pattern

Either the service grows an `auth` parameter and an internal cache (responsibilities entangle), or every caller writes its own checks (different consumers reach different conclusions about who can see what; the backend gets hammered).

See `Problem/`.

## With the Proxy pattern

`ICustomerProfileService` is implemented by:

- `RemoteCustomerProfileService` — the real, slow backend.
- `AuthorizationProxy` — a **protection proxy**.
- `CachingProxy` — a **caching proxy** with TTL.

```csharp
ICustomerProfileService service =
    new AuthorizationProxy(
        new CachingProxy(new RemoteCustomerProfileService(), TimeSpan.FromMinutes(5)),
        accessPolicy);
```

Authorization is **outside** the cache — otherwise an unauthorized request would still warm the cache and could be served on a later request from someone who shouldn't see it. The cache only ever holds responses authorized for *somebody*; the proxy still re-checks for each caller.

The trap to avoid: **proxies must remain transparent to the interface they implement**. If a caching proxy exposes `Invalidate()` outside the interface, callers depend on the proxy class directly — defeating the point.

See `Solution/`.
