using System.Reflection;
using dev.kaldiroglu.Bridge.Notifications.Domain;
using dev.kaldiroglu.Bridge.Notifications.Solution.Classic;
using dev.kaldiroglu.Bridge.Notifications.Solution.Factory;
using dev.kaldiroglu.Bridge.Notifications.Solution.Shared;
using Xunit;

namespace dev.kaldiroglu.Bridge.Tests.Notifications;

/// <summary>
/// GoF's implementation issues 2 and 3: who chooses the implementor, and what happens when
/// several abstractions share one.
/// </summary>
public class VariationsTests
{
    private static readonly Message Short = new("Order shipped", "It is on its way.");

    private readonly TransportLog _log = new();
    private readonly Transports _transports;

    public VariationsTests() => _transports = new Transports(_log);

    // ------------------------------------------------- variation 1: a factory chooses

    [Fact(DisplayName = "factory: each recipient is reached the way they asked to be")]
    public void FactoryHonorsPreference()
    {
        var service = new NotificationService(new PreferenceChannelFactory(_transports));

        var toAkin = service.Send(c => new UrgentNotification(c),
            Recipient.Of("Akin", ChannelKind.Email), Short);
        var toBora = service.Send(c => new UrgentNotification(c),
            Recipient.Of("Bora", ChannelKind.Sms), Short);
        var toCeyda = service.Send(c => new UrgentNotification(c),
            Recipient.Of("Ceyda", ChannelKind.Push), Short);

        Assert.Equal("email", toAkin.Channel);
        Assert.Equal("sms", toBora.Channel);
        Assert.Equal("push", toCeyda.Channel);

        // One call site, one notification kind, three channels — decided by a value that came out
        // of a database while the program was running. This is the test that cannot be written at
        // all against the inheritance design.
        Assert.Equal(3, _log.SendCount);
    }

    [Fact(DisplayName = "factory: the notification kind and the channel are chosen independently")]
    public void KindAndChannelAreIndependent()
    {
        var service = new NotificationService(new PreferenceChannelFactory(_transports));
        var bora = Recipient.Of("Bora", ChannelKind.Sms);

        Assert.Equal("sms", service.Send(c => new SimpleNotification(c), bora, Short).Channel);
        Assert.Equal("sms", service.Send(c => new UrgentNotification(c), bora, Short).Channel);
        Assert.Equal("sms", service.Send(c => new DigestNotification(c), bora, Short).Channel);

        // Three kinds down one channel, and the caller named neither a channel class nor a
        // combination class.
        Assert.Equal(3, _log.SendCountFor("sms"));
    }

    [Fact(DisplayName = "factory: registering a fourth channel touches no notification kind")]
    public void RegisteringAChannel()
    {
        var factory = new PreferenceChannelFactory(_transports);

        // Re-point Push at the email channel — a one-line operational change of the sort that
        // happens when a vendor goes down.
        factory.Register(ChannelKind.Push, new EmailChannel(_transports));
        var service = new NotificationService(factory);

        var result = service.Send(c => new UrgentNotification(c),
            Recipient.Of("Ceyda", ChannelKind.Push), Short);

        Assert.Equal("email", result.Channel);
        Assert.Equal(0, _log.SendCountFor("push"));
    }

    // --------------------------------------------- variation 2: sharing an implementor

    [Fact(DisplayName = "shared: one channel object serves many notifications")]
    public void OneChannelManyNotifications()
    {
        var pooled = new PooledChannel(new EmailChannel(_transports));

        List<Notification> notifications =
        [
            new SimpleNotification(pooled.Acquire()),
            new UrgentNotification(pooled.Acquire()),
            new DigestNotification(pooled.Acquire())
        ];

        var akin = Recipient.Of("Akin", ChannelKind.Email);
        notifications.ForEach(n => n.Notify(akin, Short));

        Assert.Equal(3, pooled.Users);          // three abstractions, one implementor
        Assert.Equal(3, pooled.MessagesSent);
        Assert.Equal(3, _log.SendCountFor("email"));
    }

    [Fact(DisplayName = "shared: the abstractions really do hold the same object")]
    public void SameInstance()
    {
        var pooled = new PooledChannel(new EmailChannel(_transports));
        var a = new SimpleNotification(pooled.Acquire());
        var b = new UrgentNotification(pooled.Acquire());

        // Both notifications point at one channel. That is the saving — one connection, one
        // client, one vendor session — and also the cost: shared mutable state, so it has to be
        // thread-safe, and its failures belong to everybody at once.
        Assert.Same(pooled, ExtractChannel(a));
        Assert.Same(pooled, ExtractChannel(b));
        Assert.Equal(2, pooled.Users);
    }

    [Fact(DisplayName = "shared: a pooled channel behaves exactly like the channel it wraps")]
    public void PoolingIsTransparent()
    {
        var direct = new UrgentNotification(new EmailChannel(_transports));
        var viaPool = new UrgentNotification(
            new PooledChannel(new EmailChannel(_transports)).Acquire());
        var akin = Recipient.Of("Akin", ChannelKind.Email);

        var a = direct.Notify(akin, Short);
        var b = viaPool.Notify(akin, Short);

        Assert.Equal(a.Channel, b.Channel);
        Assert.Equal(a.BodySent, b.BodySent);
        Assert.True(a.Delivered && b.Delivered);
    }

    /// <summary>Reads the implementor back out of an abstraction, for the identity check above.</summary>
    private static object? ExtractChannel(Notification notification) =>
        typeof(Notification)
            .GetField("_channel", BindingFlags.Instance | BindingFlags.NonPublic)!
            .GetValue(notification);
}
