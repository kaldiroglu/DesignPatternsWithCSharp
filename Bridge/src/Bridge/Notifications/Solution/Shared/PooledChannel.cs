using dev.kaldiroglu.Bridge.Notifications.Domain;
using dev.kaldiroglu.Bridge.Notifications.Solution.Classic;

namespace dev.kaldiroglu.Bridge.Notifications.Solution.Shared;

/// <summary>
/// Variation 2 — <b>sharing implementors</b> (GoF implementation issue 3, p. 155).
/// <para>
/// A channel is usually expensive: an SMTP connection, an HTTP client, a vendor session. Nothing
/// in the solution says every abstraction needs its own — several notifications can point at the
/// same implementor object, and usually should.
/// </para>
/// <para>
/// This wrapper makes the sharing visible. It counts how many abstractions are using it and how
/// many messages have gone through it, so a test can show one channel serving many
/// notifications.
/// </para>
/// <para>
/// <b>What it costs.</b> A shared implementor is shared state. It must be thread-safe, its
/// failures are everybody's failures, and it cannot hold anything specific to one abstraction.
/// GoF's C++ answer was reference counting; in a managed language the question is usually
/// lifecycle and thread safety instead.
/// </para>
/// </summary>
public sealed class PooledChannel : INotificationChannel
{
    private readonly INotificationChannel _delegate;
    private int _users;
    private int _messages;

    public PooledChannel(INotificationChannel channel) => _delegate = channel;

    /// <summary>Called when an abstraction starts using this channel.</summary>
    public PooledChannel Acquire()
    {
        Interlocked.Increment(ref _users);
        return this;
    }

    public int Users => Volatile.Read(ref _users);

    public int MessagesSent => Volatile.Read(ref _messages);

    public string Name => _delegate.Name;

    public string AddressOf(Recipient recipient) => _delegate.AddressOf(recipient);

    public int MaxBodyLength => _delegate.MaxBodyLength;

    public bool SupportsSubject => _delegate.SupportsSubject;

    public bool Deliver(string address, string subject, string body)
    {
        Interlocked.Increment(ref _messages);
        return _delegate.Deliver(address, subject, body);
    }
}
