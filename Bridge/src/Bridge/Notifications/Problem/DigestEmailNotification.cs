using dev.kaldiroglu.Bridge.Notifications.Domain;

namespace dev.kaldiroglu.Bridge.Notifications.Problem;

/// <summary>A third pair. Two axes, and the classes keep arriving at their product.</summary>
public sealed class DigestEmailNotification
{
    private readonly Transports _transports;

    public DigestEmailNotification(Transports transports) => _transports = transports;

    public DeliveryResult Send(Recipient to, Message m)
    {
        var combined = $"Digest: {m.Body}";
        var ok = _transports.SmtpSend(to.Email, "Your digest", combined);
        return new DeliveryResult(ok, "email", to.Email, combined, 1);
    }
}
