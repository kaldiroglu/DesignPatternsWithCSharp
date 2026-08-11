namespace dev.kaldiroglu.Decorator.Middleware.Domain;

/// <summary>A clock that moves only when something moves it.</summary>
public sealed class ManualClock : IClock
{
    private DateTimeOffset _now;

    public ManualClock(DateTimeOffset start) => _now = start;

    public DateTimeOffset Now() => _now;

    public void Advance(TimeSpan amount) => _now = _now.Add(amount);
}
