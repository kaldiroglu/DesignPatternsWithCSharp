using dev.kaldiroglu.Bridge.Notifications.Domain;

namespace dev.kaldiroglu.Bridge.Notifications.Problem;

/// <summary>
/// Naive design 3, part one: a base class that IS the email implementation.
/// <para>
/// The idea is reasonable — put the SMTP details in one place and inherit them. What it quietly
/// decides is that everything derived from it is an email, permanently.
/// </para>
/// </summary>
public abstract class EmailSender
{
    private readonly Transports _transports;

    protected EmailSender(Transports transports) => _transports = transports;

    protected bool Deliver(string address, string subject, string body) =>
        _transports.SmtpSend(address, subject, body);

    protected string ChannelName => "email";
}
