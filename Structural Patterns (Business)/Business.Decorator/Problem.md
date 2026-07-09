# Decorator — HTTP Client Pipeline

Every team needs an HTTP client, but the cross-cutting concerns vary per call site:

- Internal calls: **auth + retry on 5xx + logging**
- Third-party GET of slowly-changing data: **cache + logging**
- Health probes: **none of it**

## Without the pattern

A single `HttpClient` accumulates flags. Order of concerns is hard-coded; tests for one concern test the whole client.

See `Problem/`.

## With the Decorator pattern

`IHttpClient` interface, one `RealHttpClient`, plus decorators that each wrap an `IHttpClient` and add **one** concern. Composition expresses both *which* concerns apply and *in what order*:

```csharp
IHttpClient client =
    new LoggingDecorator(
        new CachingDecorator(
            new RetryDecorator(
                new AuthDecorator(new RealHttpClient(), () => CurrentToken()),
                3),
            TimeSpan.FromSeconds(30)));
```

**Order matters**:
- `Cache(Retry(Real))` returns cached responses without retrying. `Retry(Cache(Real))` retries the cache lookup itself.
- Auth before cache (cache key must not depend on token, but request must carry token).
- Auth before retry (otherwise an expired token gets retried as-is).

See `Solution/`.
