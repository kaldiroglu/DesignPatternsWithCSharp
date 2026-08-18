using dev.kaldiroglu.Bridge.Notifications.Domain;

namespace dev.kaldiroglu.Bridge.Notifications.Solution.Classic;

/// <summary>
/// A <b>RefinedAbstraction</b>: mark it urgent, and try again if the channel refuses it.
/// <para>
/// The retry lives here, once, and it is now true of urgent notifications over every channel
/// that exists — and over every channel added later. The <c>Problem</c> namespace needed one
/// copy of this loop per channel.
/// </para>
/// </summary>
public sealed class UrgentNotification : Notification
{
    private readonly int _maxAttempts;

    public UrgentNotification(INotificationChannel channel) : this(channel, 2)
    {
    }

    public UrgentNotification(INotificationChannel channel, int maxAttempts) : base(channel) =>
        _maxAttempts = maxAttempts;

    public override DeliveryResult Notify(Recipient to, Message message)
    {
        var marked = new Message($"URGENT {message.Subject}", message.Body);

        var result = Dispatch(to, marked, 1);
        for (var attempt = 2; attempt <= _maxAttempts && !result.Delivered; attempt++)
        {
            result = Dispatch(to, marked, attempt);
        }

        return result;
    }
}
