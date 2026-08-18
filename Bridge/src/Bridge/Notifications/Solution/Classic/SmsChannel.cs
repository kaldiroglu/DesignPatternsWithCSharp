using dev.kaldiroglu.Bridge.Notifications.Domain;

namespace dev.kaldiroglu.Bridge.Notifications.Solution.Classic;

/// <summary>
/// A <b>ConcreteImplementor</b> whose vendor has opinions: 160 characters, no subject line, and
/// it throws if you exceed either.
/// <para>
/// Those opinions are answered here, once, and are then true for every kind of notification
/// that ever gets written. In the <c>Problem</c> namespace the same rule had to be remembered
/// at several separate call sites — and was forgotten at one of them.
/// </para>
/// </summary>
public sealed class SmsChannel : INotificationChannel
{
    private readonly Transports _transports;

    public SmsChannel(Transports transports) => _transports = transports;

    public string Name => "sms";

    public string AddressOf(Recipient recipient) => recipient.Phone;

    public int MaxBodyLength => Transports.SmsLimit;

    // An SMS is one string; the abstraction folds the subject in.
    public bool SupportsSubject => false;

    public bool Deliver(string address, string subject, string body) =>
        _transports.SmsSubmit(address, body);
}
