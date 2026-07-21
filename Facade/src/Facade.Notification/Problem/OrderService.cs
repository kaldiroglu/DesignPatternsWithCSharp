using dev.kaldiroglu.Facade.Notification.Subsystems;

namespace dev.kaldiroglu.Facade.Notification.Problem;

/// <summary>
/// BEFORE the Facade pattern. Every business class that needs to notify carries
/// five subsystem dependencies, duplicates the channel-selection logic, and
/// repeats the same dispatch ceremony. This is what <c>OrderService</c>,
/// <c>PaymentService</c>, <c>UserService</c>, ... all look like.
/// </summary>
public sealed class OrderService
{
    // Five dependencies for a single side-responsibility (notifying).
    private readonly EmailService _email;
    private readonly SmsService _sms;
    private readonly SlackService _slack;
    private readonly PushNotificationService _push;
    private readonly NotificationLogger _log;

    public OrderService(
        EmailService email,
        SmsService sms,
        SlackService slack,
        PushNotificationService push,
        NotificationLogger log)
    {
        _email = email;
        _sms = sms;
        _slack = slack;
        _push = push;
        _log = log;
    }

    /// <summary>
    /// A typical business method. The notification block below (channel selection +
    /// dispatch + audit) is duplicated in every business method that notifies.
    /// </summary>
    public void ConfirmOrder(
        string userId, string? userEmail, string? userPhone, string? userSlack,
        string? userDeviceToken, bool doNotDisturb, string orderId)
    {
        // -- business logic (save to DB, etc.) --
        Console.WriteLine($"Order {orderId} confirmed.");

        // -- notification block: channel selection + dispatch + logging, by hand --
        string subject = $"Order #{orderId} Confirmed";
        string body = $"Your order #{orderId} has been confirmed.";

        if (!string.IsNullOrEmpty(userEmail))
        {
            bool ok = _email.Send(userEmail, subject, body);
            _log.Record(userId, "email", ok);
        }
        if (!string.IsNullOrEmpty(userPhone) && body.Length <= 160)
        {
            bool ok = _sms.Send(userPhone, body);
            _log.Record(userId, "sms", ok);
        }
        if (!string.IsNullOrEmpty(userSlack) && !doNotDisturb)
        {
            bool ok = _slack.Send(userSlack, body);
            _log.Record(userId, "slack", ok);
        }
        if (!string.IsNullOrEmpty(userDeviceToken))
        {
            bool ok = _push.Send(userDeviceToken, subject, body);
            _log.Record(userId, "push", ok);
        }
    }
}
