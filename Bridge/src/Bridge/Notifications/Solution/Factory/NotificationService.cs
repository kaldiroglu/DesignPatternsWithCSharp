using dev.kaldiroglu.Bridge.Notifications.Domain;
using dev.kaldiroglu.Bridge.Notifications.Solution.Classic;

namespace dev.kaldiroglu.Bridge.Notifications.Solution.Factory;

/// <summary>
/// What the application actually calls.
/// <para>
/// Note what this class does <i>not</i> contain: any channel name, any <c>switch</c>, and any
/// knowledge of who prefers what. It is handed a factory and a notification kind, and the two
/// axes meet here — for one line, at run time, and nowhere else.
/// </para>
/// </summary>
public sealed class NotificationService
{
    private readonly IChannelFactory _factory;

    public NotificationService(IChannelFactory factory) => _factory = factory;

    /// <param name="kind">
    /// How to build the notification, once the channel is known — for example
    /// <c>channel =&gt; new UrgentNotification(channel)</c>.
    /// </param>
    /// <param name="to">Who to reach.</param>
    /// <param name="message">What to say.</param>
    public DeliveryResult Send(Func<INotificationChannel, Notification> kind,
        Recipient to, Message message)
    {
        var notification = kind(_factory.ChannelFor(to));
        return notification.Notify(to, message);
    }
}
