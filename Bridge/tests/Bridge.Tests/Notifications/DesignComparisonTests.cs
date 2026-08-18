using dev.kaldiroglu.Bridge.Notifications.Domain;
using dev.kaldiroglu.Bridge.Notifications.Problem;
using dev.kaldiroglu.Bridge.Notifications.Solution.Classic;
using Xunit;

namespace dev.kaldiroglu.Bridge.Tests.Notifications;

/// <summary>
/// The naive designs and the Bridge, measured on the same wire. They agree about what goes out;
/// they disagree about what the next change costs.
/// </summary>
public class DesignComparisonTests
{
    private static readonly Message Short = new("Order shipped", "It is on its way.");
    private readonly Recipient _akin = Recipient.Of("Akin", ChannelKind.Email);

    private static TransportLog.Sent OnlySend(Action<Transports> scenario)
    {
        var log = new TransportLog();
        scenario(new Transports(log));
        Assert.Equal(1, log.SendCount);
        return log.Sends[0];
    }

    [Fact(DisplayName = "all three designs send the same urgent email")]
    public void DesignsAgree()
    {
        var bySwitch = OnlySend(t => new SwitchingNotifier(t)
            .Send(SwitchingNotifier.Kind.Urgent, ChannelKind.Email, _akin, Short));
        var byPairClass = OnlySend(t => new UrgentEmailNotification(t).Send(_akin, Short));
        var byBridge = OnlySend(t => new UrgentNotification(new EmailChannel(t))
            .Notify(_akin, Short));

        Assert.Equal(bySwitch.Channel, byBridge.Channel);
        Assert.Equal(bySwitch.Address, byBridge.Address);
        Assert.Equal(byPairClass.Channel, byBridge.Channel);
        Assert.Equal(byPairClass.Body, byBridge.Body);
    }

    [Fact(DisplayName = "a fourth channel is one class here, and every existing kind can use it")]
    public void CostOfTheNextChannel()
    {
        // The whole of a fourth channel. Nothing above it is edited, or even recompiled.
        INotificationChannel whatsApp = new WhatsAppChannel();

        // Every kind that already exists, reaching a channel written after all of them — and the
        // result names this channel, so the abstraction really did use it.
        DeliveryResult[] results =
        [
            new SimpleNotification(whatsApp).Notify(_akin, Short),
            new UrgentNotification(whatsApp).Notify(_akin, Short),
            new DigestNotification(whatsApp).Add(Short).Notify(_akin, Short)
        ];

        Assert.All(results, result =>
        {
            Assert.True(result.Delivered);
            Assert.Equal("whatsapp", result.Channel);
        });
    }

    private sealed class WhatsAppChannel : INotificationChannel
    {
        public string Name => "whatsapp";

        public string AddressOf(Recipient recipient) => recipient.Phone;

        public int MaxBodyLength => 4096;

        public bool SupportsSubject => false;

        public bool Deliver(string address, string subject, string body) => true;
    }
}
