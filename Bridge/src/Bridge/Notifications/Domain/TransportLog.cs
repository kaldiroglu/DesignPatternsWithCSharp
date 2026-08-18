namespace dev.kaldiroglu.Bridge.Notifications.Domain;

/// <summary>
/// The wire. Every raw send that any design makes goes through here, so that all the designs
/// in this project are measured on the same instrument.
/// <para>
/// Failures are <i>scripted</i>: <see cref="FailNext"/> queues up outages that the next sends
/// will hit. Nothing is random, so every test and every demo produces the same output every
/// time.
/// </para>
/// </summary>
public sealed class TransportLog
{
    /// <summary>One raw send, as the vendor SDK saw it.</summary>
    public record Sent(string Channel, string Address, string Body);

    private readonly List<Sent> _sends = [];
    private readonly Queue<bool> _scriptedFailures = new();

    public void FailNext(int count)
    {
        for (var i = 0; i < count; i++)
        {
            _scriptedFailures.Enqueue(true);
        }
    }

    /// <summary>Called by the vendor SDKs. Returns false when a scripted outage is due.</summary>
    internal bool Record(string channel, string address, string body)
    {
        _sends.Add(new Sent(channel, address, body));
        return !_scriptedFailures.TryDequeue(out _);
    }

    internal void OpenConnection() => ConnectionsOpened++;

    public IReadOnlyList<Sent> Sends => _sends.AsReadOnly();

    public int SendCount => _sends.Count;

    public int SendCountFor(string channel) => _sends.Count(s => s.Channel == channel);

    /// <summary>
    /// How many times a transport had to be opened. Matters for the shared-implementor
    /// variation.
    /// </summary>
    public int ConnectionsOpened { get; private set; }

    public void Reset()
    {
        _sends.Clear();
        _scriptedFailures.Clear();
        ConnectionsOpened = 0;
    }
}
