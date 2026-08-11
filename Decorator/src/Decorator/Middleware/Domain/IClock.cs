using System.Globalization;

namespace dev.kaldiroglu.Decorator.Middleware.Domain;

/// <summary>Time, as an interface, so the tests never wait for it.</summary>
public interface IClock
{
    DateTimeOffset Now();

    static ManualClock Manual() =>
        new(DateTimeOffset.Parse("2026-07-27T09:00:00Z", CultureInfo.InvariantCulture));

    static IClock System() => new SystemClock();

    private sealed class SystemClock : IClock
    {
        public DateTimeOffset Now() => DateTimeOffset.UtcNow;
    }
}
