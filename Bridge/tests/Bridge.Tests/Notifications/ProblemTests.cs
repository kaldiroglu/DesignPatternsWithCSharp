using dev.kaldiroglu.Bridge.Notifications.Domain;
using dev.kaldiroglu.Bridge.Notifications.Problem;
using dev.kaldiroglu.Bridge.Notifications.Solution.Classic;
using Xunit;

namespace dev.kaldiroglu.Bridge.Tests.Notifications;

/// <summary>
/// The three naive designs. All of them work — that is what makes them worth teaching.
/// </summary>
public class ProblemTests
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

    public ProblemTests() => _transports = new Transports(_log);

    // ------------------------------------------------- design 1: switch on both axes

    [Fact(DisplayName = "switch: it works, over every channel")]
    public void SwitchingWorks()
    {
        var notifier = new SwitchingNotifier(_transports);

        Assert.True(notifier.Send(SwitchingNotifier.Kind.Simple, ChannelKind.Email, _akin, Short).Delivered);
        Assert.True(notifier.Send(SwitchingNotifier.Kind.Urgent, ChannelKind.Sms, _akin, Short).Delivered);
        Assert.True(notifier.Send(SwitchingNotifier.Kind.Simple, ChannelKind.Push, _akin, Short).Delivered);
        Assert.Equal(3, _log.SendCount);
    }

    [Fact(DisplayName = "switch: three branches send an SMS and two state the rule, so one forgot")]
    public void TheForgottenRule()
    {
        var notifier = new SwitchingNotifier(_transports);

        // Simple and urgent remembered the 160-character limit...
        Assert.True(notifier.Send(SwitchingNotifier.Kind.Simple, ChannelKind.Sms, _akin, Long).Delivered);

        // ...and digest did not. The transport throws, in production, at 2am. Nobody deleted the
        // rule: the person who added digests never knew it existed, because it was not written
        // anywhere that a new branch would have to look.
        Assert.Throws<ArgumentException>(() =>
            notifier.Send(SwitchingNotifier.Kind.Digest, ChannelKind.Sms, _akin, Long));
    }

    [Fact(DisplayName = "switch: nine branches by hand, and the SMS rule written in two of three")]
    public void BranchesMultiply()
    {
        var source = File.ReadAllText(Path.Combine(
            TypeCensus.SourceRoot, "Notifications", "Problem", "SwitchingNotifier.cs"));
        var body = source[source.IndexOf("public sealed class", StringComparison.Ordinal)..];

        var leaves = TypeCensus.CountOf(body, "case ChannelKind.Email:")
                     + TypeCensus.CountOf(body, "case ChannelKind.Sms:")
                     + TypeCensus.CountOf(body, "case ChannelKind.Push:");
        Assert.Equal(9, leaves);   // branches, one per pair, written by hand

        Assert.Equal(3, TypeCensus.CountOf(body, "case ChannelKind.Sms:"));
        Assert.Equal(2, TypeCensus.CountOf(body, "Transports.SmsLimit"));

        // The push limit, by contrast, is remembered in all three of its branches. The design
        // does not fail reliably; it fails wherever somebody happened to forget.
        Assert.Equal(3, TypeCensus.CountOf(body, "case ChannelKind.Push:"));
        Assert.Equal(3, TypeCensus.CountOf(body, "Transports.PushLimit"));
    }

    // ------------------------------------------- design 2: a class per (kind, channel)

    [Fact(DisplayName = "class per pair: it works, and the retry is written once per channel")]
    public void ClassPerPairWorks()
    {
        var onEmail = new UrgentEmailNotification(_transports).Send(_akin, Short);
        var onSms = new UrgentSmsNotification(_transports).Send(_akin, Short);

        Assert.True(onEmail.Delivered);
        Assert.True(onSms.Delivered);
        Assert.Equal("email", onEmail.Channel);
        Assert.Equal("sms", onSms.Channel);

        // Both classes contain the same retry loop. It is the only thing that makes a
        // notification urgent, and it now exists in two places that cannot share it.
        Assert.Equal(2, _log.SendCount);
    }

    [Fact(DisplayName = "class per pair: a retried failure costs two sends, in both copies")]
    public void BothCopiesRetry()
    {
        _log.FailNext(1);
        Assert.Equal(2, new UrgentEmailNotification(_transports).Send(_akin, Short).Attempts);

        _log.FailNext(1);
        Assert.Equal(2, new UrgentSmsNotification(_transports).Send(_akin, Short).Attempts);
    }

    [Fact(DisplayName = "class per pair: the class count is the product of the two axes")]
    public void ClassesMultiply()
    {
        var kinds = TypeCensus.ConcreteImplementationsOf(ClassicNs, typeof(Notification));
        var channels = TypeCensus.ConcreteImplementationsOf(ClassicNs, typeof(INotificationChannel));

        Assert.Equal(3, kinds);
        Assert.Equal(3, channels);

        Assert.Equal(9, kinds * channels);          // classes a one-per-pair design must write
        Assert.Equal(12, kinds * (channels + 1));   // a fourth channel: three more
        Assert.Equal(12, (kinds + 1) * channels);   // a fourth kind: three more

        // And the same two axes cost kinds + channels in the bridge design.
        Assert.Equal(6, kinds + channels);
    }

    // ------------------------------------------------ design 3: inherit the channel

    [Fact(DisplayName = "inherit the channel: it works, and it ignores what the user asked for")]
    public void InheritanceIgnoresPreference()
    {
        var prefersSms = Recipient.Of("Bora", ChannelKind.Sms);
        var result = new EmailBoundUrgentNotification(_transports).Send(prefersSms, Short);

        Assert.True(result.Delivered);
        Assert.Equal(ChannelKind.Sms, prefersSms.Preferred);
        Assert.Equal("email", result.Channel);      // sent the wrong way, successfully
        Assert.NotEqual(prefersSms.Preferred.ToString().ToLowerInvariant(), result.Channel);
    }

    [Fact(DisplayName = "inherit the channel: the channel is the type, so it cannot be chosen at run time")]
    public void ChannelIsTheType()
    {
        // EmailBoundUrgentNotification IS an EmailSender. There is no operation, no setter and
        // no configuration that can make this object send an SMS — a base class is chosen when
        // the code is compiled, and never again.
        Assert.True(typeof(EmailSender).IsAssignableFrom(typeof(EmailBoundUrgentNotification)));
        Assert.DoesNotContain(typeof(EmailBoundUrgentNotification).GetMethods(),
            m => m.Name.Contains("SetChannel"));
    }

    [Fact(DisplayName = "all three designs put the same messages on the wire")]
    public void AllThreeWork()
    {
        new UrgentEmailNotification(_transports).Send(_akin, Short);
        new DigestEmailNotification(_transports).Send(_akin, Short);
        new EmailBoundUrgentNotification(_transports).Send(_akin, Short);
        new SwitchingNotifier(_transports)
            .Send(SwitchingNotifier.Kind.Simple, ChannelKind.Email, _akin, Short);

        Assert.Equal(4, _log.SendCountFor("email"));
    }
}
