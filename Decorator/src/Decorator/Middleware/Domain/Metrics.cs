namespace dev.kaldiroglu.Decorator.Middleware.Domain;

/// <summary>Where timings go.</summary>
public sealed class Metrics
{
    /// <summary>One measured call.</summary>
    public sealed record Sample(string Sku, TimeSpan Elapsed);

    private readonly List<Sample> _samples = [];

    public void Record(string sku, TimeSpan elapsed) => _samples.Add(new Sample(sku, elapsed));

    public IReadOnlyList<Sample> Samples() => _samples.AsReadOnly();

    public int Size => _samples.Count;

    public TimeSpan Slowest() =>
        _samples.Count == 0 ? TimeSpan.Zero : _samples.Max(s => s.Elapsed);
}
