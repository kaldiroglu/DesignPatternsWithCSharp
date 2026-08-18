using dev.kaldiroglu.Bridge.Notifications.Domain;
using dev.kaldiroglu.Bridge.Notifications.Solution.Classic;
using Xunit;

namespace dev.kaldiroglu.Bridge.Tests.Notifications;

/// <summary>
/// The Bridge itself: three notification kinds over three channels, from six classes.
/// </summary>
public class ClassicBridgeTests
{
    private const string ClassicNs = "dev.kaldiroglu.Bridge.Notifications.Solution.Classic";

    private static readonly Message Short = new("Order shipped", "It is on its way.");

    private static readonly Message Long = new("Payment failed",
        "We could not take payment for order 4021. Please update the card on file. "
        + "If the payment is not completed within 48 hours the order will be released "
        + "and the reserved stock returned to the warehouse for other customers.");

    private readonly TransportLog _log = new();
    private readonly Transports _transports;
    private readonly Recipient _akin = Recipient.Of("Akin", ChannelKind.Email);

    public ClassicBridgeTests() => _transports = new Transports(_log);

    private List<INotificationChannel> AllChannels() =>
    [
        new EmailChannel(_transports), new SmsChannel(_transports), new PushChannel(_transports)
    ];

    [Fact(DisplayName = "every kind works over every channel — 3 x 3, from 3 + 3 classes")]
    public void EveryKindOverEveryChannel()
    {
        foreach (var channel in AllChannels())
        {
            Assert.True(new SimpleNotification(channel).Notify(_akin, Short).Delivered);
            Assert.True(new UrgentNotification(channel).Notify(_akin, Short).Delivered);
            Assert.True(new DigestNotification(channel).Add(Short).Notify(_akin, Short).Delivered);
        }

        Assert.Equal(9, _log.SendCount);
    }

    [Fact(DisplayName = "the retry lives in one place and is true over every channel")]
    public void RetryIsWrittenOnce()
    {
        foreach (var channel in AllChannels())
        {
            _log.FailNext(1);
            Assert.Equal(2, new UrgentNotification(channel).Notify(_akin, Short).Attempts);
        }

        // Three channels, one retry loop, six sends: two attempts on each.
        Assert.Equal(6, _log.SendCount);
    }

    [Fact(DisplayName = "the channel's limit is asked for, not assumed — and every kind respects it")]
    public void TheAbstractionAsksForLimits()
    {
        var onEmail = new UrgentNotification(new EmailChannel(_transports)).Notify(_akin, Long);
        var onSms = new UrgentNotification(new SmsChannel(_transports)).Notify(_akin, Long);
        var onPush = new UrgentNotification(new PushChannel(_transports)).Notify(_akin, Long);

        Assert.False(onEmail.Truncated(Long.Body));
        Assert.Equal(Transports.SmsLimit, onSms.BodySent.Length);
        Assert.Equal(Transports.PushLimit, onPush.BodySent.Length);

        // The digest — the kind that forgot the rule in the Problem namespace — gets it for free
        // here, because the rule belongs to the channel rather than to a call site.
        var digest = new DigestNotification(new SmsChannel(_transports))
            .Add(Long).Add(Long).Notify(_akin, Long);
        Assert.True(digest.Delivered);
        Assert.Equal(Transports.SmsLimit, digest.BodySent.Length);
    }

    [Fact(DisplayName = "a channel without a subject line gets one folded into the body")]
    public void ChannelsWithoutSubjects()
    {
        var onSms = new SimpleNotification(new SmsChannel(_transports)).Notify(_akin, Short);

        // The abstraction asked SupportsSubject. It did not ask what kind of channel this is,
        // and there is no type test anywhere in the Solution namespace.
        Assert.StartsWith("Order shipped: ", onSms.BodySent);
    }

    [Fact(DisplayName = "the channel can be swapped on a notification that already exists")]
    public void ChannelIsChosenAtRunTime()
    {
        Notification notification = new UrgentNotification(new EmailChannel(_transports));
        Assert.Equal("email", notification.ChannelName);
        notification.Notify(_akin, Short);

        notification.SetChannel(new SmsChannel(_transports));    // same object

        Assert.Equal("sms", notification.ChannelName);
        notification.Notify(_akin, Short);
        Assert.Equal(1, _log.SendCountFor("email"));
        Assert.Equal(1, _log.SendCountFor("sms"));
    }

    [Fact(DisplayName = "a refined abstraction may hold state and still know nothing about channels")]
    public void RefinementsMayHaveState()
    {
        var digest = new DigestNotification(new EmailChannel(_transports));
        digest.Add(new Message("A", "first")).Add(new Message("B", "second"));
        Assert.Equal(2, digest.PendingCount);
        Assert.Equal(0, _log.SendCount);          // nothing sent yet

        var result = digest.Flush(_akin);
        Assert.Equal(1, _log.SendCount);          // one message carrying both
        Assert.Contains("first", result.BodySent);
        Assert.Contains("second", result.BodySent);
        Assert.Equal(0, digest.PendingCount);
    }

    [Fact(DisplayName = "a new channel costs one class and works with every kind at once")]
    public void AddingAChannelCostsOneClass()
    {
        // A fax channel, invented here in a dozen lines. No notification kind is touched, and
        // all three of them can use it immediately.
        INotificationChannel fax = new FaxChannel();

        Assert.True(new SimpleNotification(fax).Notify(_akin, Short).Delivered);
        Assert.True(new UrgentNotification(fax).Notify(_akin, Short).Delivered);
        Assert.True(new DigestNotification(fax).Add(Short).Notify(_akin, Short).Delivered);
    }

    [Fact(DisplayName = "a new kind costs one class and works over every channel at once")]
    public void AddingAKindCostsOneClass()
    {
        // A "quiet" notification that never retries and always trims hard, written once.
        foreach (var channel in AllChannels())
        {
            Assert.True(new QuietNotification(channel).Notify(_akin, Long).Delivered);
        }

        Assert.Equal(3, _log.SendCount);
    }

    [Fact(DisplayName = "class arithmetic: kinds + channels, counted from the namespace itself")]
    public void TheArithmetic()
    {
        var kinds = TypeCensus.ConcreteImplementationsOf(ClassicNs, typeof(Notification));
        var channels = TypeCensus.ConcreteImplementationsOf(ClassicNs, typeof(INotificationChannel));

        Assert.Equal(3, kinds);       // refined abstractions
        Assert.Equal(3, channels);    // concrete implementors

        // The slides' headline pair. m + n is what the solution replaces m x n with, and the two
        // roots are the overhead it charges for doing so.
        Assert.Equal(6, kinds + channels);   // the classes that carry the two axes
        Assert.Equal(9, kinds * channels);   // the grid a class-per-pair design writes out
        Assert.Equal(8, TypeCensus.In(ClassicNs).Count);   // every type on the design diagram

        // And the two roots really are exactly two, so that last figure is not a guess.
        Assert.Contains(typeof(Notification), TypeCensus.In(ClassicNs));
        Assert.Contains(typeof(INotificationChannel), TypeCensus.In(ClassicNs));
    }

    [Fact(DisplayName = "a notification needs a channel")]
    public void NullChannelIsRejected() =>
        Assert.Throws<ArgumentNullException>(() => new SimpleNotification(null!));

    private sealed class FaxChannel : INotificationChannel
    {
        public string Name => "fax";

        public string AddressOf(Recipient recipient) => recipient.Phone;

        public int MaxBodyLength => 2000;

        public bool SupportsSubject => true;

        public bool Deliver(string address, string subject, string body) => true;
    }

    private sealed class QuietNotification : Notification
    {
        public QuietNotification(INotificationChannel channel) : base(channel)
        {
        }

        public override DeliveryResult Notify(Recipient to, Message message) =>
            Dispatch(to, new Message(message.Subject, Fit(message.Body)), 1);
    }
}
