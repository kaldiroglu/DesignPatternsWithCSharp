using dev.kaldiroglu.Facade.Notification.Subsystems;

namespace dev.kaldiroglu.Facade.Notification.Solution;

/// <summary>
/// <b>Facade</b> — a single, unified interface over four notification subsystems
/// plus audit logging. Responsibilities it absorbs from every client:
/// <list type="number">
///   <item>Subsystem construction &amp; credential management</item>
///   <item>Channel selection (email present? SMS fits 160 chars? DND?)</item>
///   <item>Per-channel audit logging</item>
///   <item>Error isolation (one channel failing does not crash the others)</item>
/// </list>
/// Clients depend on this one class and call <see cref="Notify"/>; they never
/// touch <see cref="EmailService"/>, <see cref="SmsService"/>, etc.
/// </summary>
public sealed class NotificationFacade
{
    private readonly EmailService _email;
    private readonly SmsService _sms;
    private readonly SlackService _slack;
    private readonly PushNotificationService _push;
    private readonly NotificationLogger _log;

    /// <summary>One place to wire up every subsystem, from a single config object.</summary>
    public NotificationFacade(NotificationConfig config)
    {
        _email = new EmailService(config.SmtpHost, config.SmtpPort, config.SmtpUsername, config.SmtpPassword);
        _sms = new SmsService(config.TwilioSid, config.TwilioToken, config.TwilioFromNumber);
        _slack = new SlackService(config.SlackWebhookUrl);
        _push = new PushNotificationService(config.FirebaseCredentialsPath);
        _log = new NotificationLogger(config.LogFilePath);
    }

    /// <summary>The single method every client calls. One line replaces ~20.</summary>
    public NotificationResult Notify(User user, string title, string body)
    {
        var result = new NotificationResult();

        if (user.Email is not null)
        {
            TryChannel(result, user.Id, "email", () => _email.Send(user.Email, title, body));
        }
        if (user.Phone is not null && body.Length <= 160)
        {
            TryChannel(result, user.Id, "sms", () => _sms.Send(user.Phone, body));
        }
        if (user.SlackChannel is not null && !user.DoNotDisturb)
        {
            TryChannel(result, user.Id, "slack", () => _slack.Send(user.SlackChannel, body));
        }
        if (user.DeviceToken is not null)
        {
            TryChannel(result, user.Id, "push", () => _push.Send(user.DeviceToken, title, body));
        }

        return result;
    }

    // Error isolation: a failing channel is recorded as a failure, not propagated.
    private void TryChannel(NotificationResult result, string userId, string channel, Func<bool> send)
    {
        try
        {
            bool ok = send();
            result.Record(channel, ok);
            _log.Record(userId, channel, ok);
        }
        catch
        {
            result.Record(channel, false);
        }
    }
}
