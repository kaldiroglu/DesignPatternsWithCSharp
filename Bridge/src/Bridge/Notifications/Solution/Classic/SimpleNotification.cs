using dev.kaldiroglu.Bridge.Notifications.Domain;

namespace dev.kaldiroglu.Bridge.Notifications.Solution.Classic;

/// <summary>A <b>RefinedAbstraction</b>: say it once, over whatever channel is in use.</summary>
public sealed class SimpleNotification : Notification
{
    public SimpleNotification(INotificationChannel channel) : base(channel)
    {
    }

    public override DeliveryResult Notify(Recipient to, Message message) =>
        Dispatch(to, message, 1);
}
