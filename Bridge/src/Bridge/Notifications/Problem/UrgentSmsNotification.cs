using dev.kaldiroglu.Bridge.Notifications.Domain;

namespace dev.kaldiroglu.Bridge.Notifications.Problem;

/// <summary>
/// The same "urgent" idea, over a different channel.
/// <para>
/// Put this class beside <see cref="UrgentEmailNotification"/> and read them together. The
/// retry — the thing that makes a notification urgent — is written twice, and it will be
/// written a third time when push arrives.
/// </para>
/// </summary>
public sealed class UrgentSmsNotification
{
    private readonly Transports _transports;

    public UrgentSmsNotification(Transports transports) => _transports = transports;

    public DeliveryResult Send(Recipient to, Message m)
    {
        var text = $"URGENT {m.Subject}: {m.Body}";
        if (text.Length > Transports.SmsLimit)
        {
            text = text[..Transports.SmsLimit];
        }

        var ok = _transports.SmsSubmit(to.Phone, text);
        var attempts = 1;
        if (!ok)
        {
            ok = _transports.SmsSubmit(to.Phone, text);
            attempts = 2;
        }

        return new DeliveryResult(ok, "sms", to.Phone, text, attempts);
    }
}
