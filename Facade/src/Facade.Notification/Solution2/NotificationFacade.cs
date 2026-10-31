using dev.kaldiroglu.Facade.Notification.Solution;
using dev.kaldiroglu.Facade.Notification.Subsystems;

namespace dev.kaldiroglu.Facade.Notification.Solution2;

/// <summary>
/// Facade with channel selection: clients pick which channels, and the facade still owns how
/// to send, the constraints and the error handling. Two overloads:
/// <c>Notify(user, title, body)</c> means all channels, and
/// <c>Notify(user, channels, title, body)</c> lets the caller pick the subset.
/// </summary>
public sealed class NotificationFacade
{
    private readonly EmailService _email;
    private readonly SmsService _sms;
    private readonly SlackService _slack;
    private readonly PushNotificationService _push;
    private readonly NotificationLogger _log;

    public NotificationFacade(NotificationConfig config)
    {
        _email = new EmailService(config.SmtpHost, config.SmtpPort, config.SmtpUsername, config.SmtpPassword);
        _sms = new SmsService(config.TwilioSid, config.TwilioToken, config.TwilioFromNumber);
        _slack = new SlackService(config.SlackWebhookUrl);
        _push = new PushNotificationService(config.FirebaseCredentialsPath);
        _log = new NotificationLogger(config.LogFilePath);
    }

    /// <summary>Convenience: all channels.</summary>
    public NotificationResult Notify(User user, string title, string body) =>
        Notify(user, Enum.GetValues<Channel>().ToHashSet(), title, body);

    /// <summary>Channel-aware: the caller picks the subset.</summary>
    public NotificationResult Notify(User user, IReadOnlySet<Channel> channels, string title, string body)
    {
        var result = new NotificationResult();

        if (channels.Contains(Channel.Email) && user.Email is not null)
        {
            TryChannel(result, user.Id, "email", () => _email.Send(user.Email, title, body));
        }

        if (channels.Contains(Channel.Sms) && user.Phone is not null)
        {
            // A body the SMS gateway cannot carry is not sent, and the result says so.
            TryChannel(result, user.Id, "sms", () => body.Length <= 160 && _sms.Send(user.Phone, body));
        }

        if (channels.Contains(Channel.Slack) && user.SlackChannel is not null && !user.DoNotDisturb)
        {
            TryChannel(result, user.Id, "slack", () => _slack.Send(user.SlackChannel, body));
        }

        if (channels.Contains(Channel.Push) && user.DeviceToken is not null)
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
