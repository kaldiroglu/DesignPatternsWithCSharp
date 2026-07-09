namespace DevKaldiroglu.DP.Structural.Bridge.Solution;

public sealed record Recipient(string Email, string Phone, string SlackChannel, string PushDeviceToken);
public sealed record RenderedMessage(string Subject, string Body);

public interface IDeliveryChannel
{
    bool SupportsRichContent { get; }
    int MaxBodyLength { get; }
    void Deliver(Recipient recipient, RenderedMessage message);
}

public class EmailChannel : IDeliveryChannel
{
    public bool SupportsRichContent => true;
    public int MaxBodyLength => 1_000_000;
    public void Deliver(Recipient r, RenderedMessage m) =>
        Console.WriteLine($"EMAIL to={r.Email} subject='{m.Subject}' body='{m.Body}'");
}

public class SmsChannel : IDeliveryChannel
{
    public bool SupportsRichContent => false;
    public int MaxBodyLength => 160;
    public void Deliver(Recipient r, RenderedMessage m) =>
        Console.WriteLine($"SMS to={r.Phone} text='{m.Body}'");
}

public class SlackChannel : IDeliveryChannel
{
    public bool SupportsRichContent => true;
    public int MaxBodyLength => 40_000;
    public void Deliver(Recipient r, RenderedMessage m) =>
        Console.WriteLine($"SLACK to={r.SlackChannel} blocks=[{m.Subject} | {m.Body}]");
}

public abstract class Notification
{
    protected readonly IDeliveryChannel Channel;
    protected Notification(IDeliveryChannel channel) => Channel = channel;

    protected abstract RenderedMessage Render(IDeliveryChannel channel);

    public void Send(Recipient recipient)
    {
        var rendered = Render(Channel);
        if (rendered.Body.Length > Channel.MaxBodyLength)
            throw new InvalidOperationException(
                $"Renderer overflowed channel limit: {rendered.Body.Length} > {Channel.MaxBodyLength}");
        Channel.Deliver(recipient, rendered);
    }
}

public class OrderConfirmation : Notification
{
    private readonly string _orderId;
    private readonly IReadOnlyList<string> _lineItems;
    private readonly decimal _total;

    public OrderConfirmation(IDeliveryChannel channel, string orderId, IReadOnlyList<string> lineItems, decimal total)
        : base(channel)
    {
        _orderId = orderId; _lineItems = lineItems; _total = total;
    }

    protected override RenderedMessage Render(IDeliveryChannel channel)
    {
        var subject = $"Order {_orderId} confirmed";
        if (channel.SupportsRichContent)
        {
            var items = string.Join("</li><li>", _lineItems);
            return new RenderedMessage(subject,
                $"<h1>Thanks!</h1><ul><li>{items}</li></ul><p>Total: ${_total}</p>");
        }
        return new RenderedMessage(subject, $"Order {_orderId} confirmed. Total ${_total}");
    }
}

public class PasswordReset : Notification
{
    private readonly string _resetLink;
    public PasswordReset(IDeliveryChannel channel, string resetLink) : base(channel) => _resetLink = resetLink;

    protected override RenderedMessage Render(IDeliveryChannel channel)
    {
        var subject = "Reset your password";
        return channel.SupportsRichContent
            ? new RenderedMessage(subject, $"<a href=\"{_resetLink}\">Click to reset</a>")
            : new RenderedMessage(subject, $"Reset: {_resetLink}");
    }
}

public static class SolutionDemo
{
    public static void Run()
    {
        var ada = new Recipient("ada@example.com", "+15555550100", "#sales", "tok-abc");

        new OrderConfirmation(new EmailChannel(), "ORD-1", new[] { "Widget", "Gadget" }, 49.95m).Send(ada);
        new OrderConfirmation(new SmsChannel(),   "ORD-1", new[] { "Widget", "Gadget" }, 49.95m).Send(ada);
        new OrderConfirmation(new SlackChannel(), "ORD-1", new[] { "Widget", "Gadget" }, 49.95m).Send(ada);

        new PasswordReset(new EmailChannel(), "https://example.com/reset/abc").Send(ada);
        new PasswordReset(new SmsChannel(),   "https://example.com/reset/abc").Send(ada);
    }
}
