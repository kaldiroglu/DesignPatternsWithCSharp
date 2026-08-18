using dev.kaldiroglu.Bridge.Notifications.Domain;

namespace dev.kaldiroglu.Bridge.Notifications.Problem;

/// <summary>
/// Naive design 2: a class for each (kind, channel) pair.
/// <para>
/// Cleaner than the switch — each class is short and does one thing. And it is exactly the
/// design GoF draw on p. 151, with exactly the same fault: the class name has to name both
/// axes, so the number of classes is their product.
/// </para>
/// </summary>
public sealed class UrgentEmailNotification
{
    private readonly Transports _transports;

    public UrgentEmailNotification(Transports transports) => _transports = transports;

    public DeliveryResult Send(Recipient to, Message m)
    {
        var ok = _transports.SmtpSend(to.Email, m.Subject, m.Body);
        var attempts = 1;
        if (!ok)
        {
            ok = _transports.SmtpSend(to.Email, m.Subject, m.Body);
            attempts = 2;
        }

        return new DeliveryResult(ok, "email", to.Email, m.Body, attempts);
    }
}
