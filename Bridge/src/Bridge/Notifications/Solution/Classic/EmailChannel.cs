using dev.kaldiroglu.Bridge.Notifications.Domain;

namespace dev.kaldiroglu.Bridge.Notifications.Solution.Classic;

/// <summary>
/// A <b>ConcreteImplementor</b>. It knows one vendor SDK and nothing else — not what an urgent
/// notification is, not what a digest is, and it will never need to change when either of them
/// changes.
/// </summary>
public sealed class EmailChannel : INotificationChannel
{
    private readonly Transports _transports;

    public EmailChannel(Transports transports) => _transports = transports;

    public string Name => "email";

    public string AddressOf(Recipient recipient) => recipient.Email;

    public int MaxBodyLength => Transports.EmailLimit;

    public bool SupportsSubject => true;

    public bool Deliver(string address, string subject, string body) =>
        _transports.SmtpSend(address, subject, body);
}
