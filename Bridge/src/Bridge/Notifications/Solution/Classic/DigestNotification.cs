using dev.kaldiroglu.Bridge.Notifications.Domain;

namespace dev.kaldiroglu.Bridge.Notifications.Solution.Classic;

/// <summary>
/// A <b>RefinedAbstraction</b> that holds real state: collect several messages, then send one.
/// <para>
/// It is worth showing because it proves the abstraction hierarchy is a hierarchy and not
/// decoration. This class has its own data and its own lifecycle, and still says nothing about
/// any channel.
/// </para>
/// </summary>
public sealed class DigestNotification : Notification
{
    private readonly List<Message> _pending = [];

    public DigestNotification(INotificationChannel channel) : base(channel)
    {
    }

    /// <summary>Add a message to the digest. Nothing is sent yet.</summary>
    public DigestNotification Add(Message message)
    {
        _pending.Add(message);
        return this;
    }

    public int PendingCount => _pending.Count;

    public override DeliveryResult Notify(Recipient to, Message message)
    {
        Add(message);
        return Flush(to);
    }

    /// <summary>Send everything collected so far as a single message.</summary>
    public DeliveryResult Flush(Recipient to)
    {
        var body = string.Join(" | ", _pending.Select(m => m.Body));
        var combined = new Message($"Your digest ({_pending.Count})", body);
        _pending.Clear();
        // Fit() is inherited: whatever the channel's limit is, the digest respects it.
        return Dispatch(to, combined, 1);
    }
}
