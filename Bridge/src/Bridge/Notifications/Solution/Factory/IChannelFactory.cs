using dev.kaldiroglu.Bridge.Notifications.Domain;
using dev.kaldiroglu.Bridge.Notifications.Solution.Classic;

namespace dev.kaldiroglu.Bridge.Notifications.Solution.Factory;

/// <summary>
/// Variation 1 — <b>who chooses the implementor?</b>
/// <para>
/// GoF raise this as implementation issue 2 (p. 155): "How, where, and when do you decide which
/// Implementor class to instantiate?" In the classic version the caller does it by writing
/// <c>new SmsChannel(..)</c> — which means every caller now knows the names of every channel,
/// and the choice is made where the notification is created rather than where the knowledge
/// lives.
/// </para>
/// <para>
/// A factory moves that decision to one place. The abstraction never names a channel again.
/// </para>
/// </summary>
public interface IChannelFactory
{
    /// <summary>The channel this recipient should be reached on, right now.</summary>
    INotificationChannel ChannelFor(Recipient recipient);
}
