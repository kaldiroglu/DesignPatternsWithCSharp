namespace dev.kaldiroglu.Facade.Notification.Solution;

/// <summary>Aggregated result of a <see cref="NotificationFacade.Notify"/> call — which channels succeeded or failed.</summary>
public sealed class NotificationResult
{
    private readonly Dictionary<string, bool> _results = new();

    public void Record(string channel, bool success) => _results[channel] = success;

    /// <summary>True when every attempted channel succeeded (vacuously true if none were attempted).</summary>
    public bool AllSucceeded => _results.Values.All(ok => ok);

    public IReadOnlyDictionary<string, bool> Results => _results;

    public override string ToString() =>
        "NotificationResult{" + string.Join(", ", _results.Select(kv => $"{kv.Key}={kv.Value}")) + "}";
}
