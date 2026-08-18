using dev.kaldiroglu.Bridge.Notifications.Domain;

namespace dev.kaldiroglu.Bridge.Notifications.Solution.Classic;

/// <summary>
/// The <b>Abstraction</b>: what a notification is, to the rest of the application.
/// <para>
/// One field, and it is the entire solution. This class does not extend a channel — it
/// <i>holds</i> one, and it can be handed a different one at any moment, including after it has
/// been constructed.
/// </para>
/// <para>
/// Read <see cref="Dispatch"/> carefully. It is written entirely in terms of the implementor's
/// primitives: ask the channel for the address, ask it how much text it will take, ask it
/// whether it has a subject line. It never asks <i>which</i> channel it is talking to, and
/// there is no <c>if (channel is SmsChannel)</c> anywhere in this namespace. The moment such a
/// line appears, the bridge is gone.
/// </para>
/// </summary>
public abstract class Notification
{
    private INotificationChannel _channel;

    protected Notification(INotificationChannel channel) =>
        _channel = channel ?? throw new ArgumentNullException(
            nameof(channel), "a notification needs a channel");

    /// <summary>The channel currently in use.</summary>
    protected INotificationChannel Channel => _channel;

    public string ChannelName => _channel.Name;

    /// <summary>
    /// Point this notification at a different channel.
    /// <para>
    /// Nothing in the <c>Problem</c> namespace can do this: there, the channel is the class, so
    /// a user who changes their preference needs a different object.
    /// </para>
    /// </summary>
    public void SetChannel(INotificationChannel newChannel) =>
        _channel = newChannel ?? throw new ArgumentNullException(nameof(newChannel));

    /// <summary>What this <i>kind</i> of notification does. The refinements below define it.</summary>
    public abstract DeliveryResult Notify(Recipient to, Message message);

    // --- the higher-level operation, built from primitives --------------------------

    /// <summary>
    /// Hand one message to whatever channel is in use, shaped to fit it.
    /// <para>
    /// This is the method every refinement is written on top of, and it is the reason the SMS
    /// length rule is stated exactly once in this design.
    /// </para>
    /// </summary>
    protected DeliveryResult Dispatch(Recipient to, Message message, int attempts)
    {
        var address = _channel.AddressOf(to);
        var body = _channel.SupportsSubject
            ? message.Body
            : $"{message.Subject}: {message.Body}";
        body = Fit(body);

        var delivered = _channel.Deliver(address, message.Subject, body);
        return new DeliveryResult(delivered, _channel.Name, address, body, attempts);
    }

    /// <summary>Trim a body to whatever the channel in use will carry.</summary>
    protected string Fit(string body)
    {
        var limit = _channel.MaxBodyLength;
        return body.Length <= limit ? body : body[..limit];
    }
}
