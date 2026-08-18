using dev.kaldiroglu.Bridge.Notifications.Domain;

namespace dev.kaldiroglu.Bridge.Notifications.Solution.Classic;

/// <summary>
/// A third <b>ConcreteImplementor</b>. Adding it cost one class and touched nothing else.
/// </summary>
public sealed class PushChannel : INotificationChannel
{
    private readonly Transports _transports;

    public PushChannel(Transports transports) => _transports = transports;

    public string Name => "push";

    public string AddressOf(Recipient recipient) => recipient.DeviceToken;

    public int MaxBodyLength => Transports.PushLimit;

    public bool SupportsSubject => false;

    public bool Deliver(string address, string subject, string body) =>
        _transports.PushNotify(address, body);
}
