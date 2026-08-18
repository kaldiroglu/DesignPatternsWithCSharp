using dev.kaldiroglu.Bridge.Notifications.Domain;

namespace dev.kaldiroglu.Bridge.Notifications.Problem;

/// <summary>
/// Naive design 3, part two: the notification kind inherits the channel.
/// <para>
/// This is the design that feels cleanest of the three, and it is the one that hurts most,
/// because of what its type says. An <c>EmailBoundUrgentNotification</c> is an
/// <see cref="EmailSender"/>. Not "has a channel" — it is one. So the channel is fixed when the
/// class is compiled, and a user whose stored preference is SMS cannot be served by this object
/// at any price; the retry logic — the only part that is really about urgency — is trapped
/// inside an email class, and the SMS version will have to copy it; and a second channel is not
/// a new class here, it is a new class <i>per kind</i>.
/// </para>
/// <para>
/// The symptom is one silent email. The disease is that avoiding the mismatch is now every
/// caller's job: the preference is readable, so each call site has to remember to read it and
/// branch, and no compiler will notice the site that forgot.
/// </para>
/// <para>
/// GoF put it plainly (p. 151): "it makes client code platform-dependent" and "makes it hard to
/// extend the abstraction and its implementation independently".
/// </para>
/// </summary>
public sealed class EmailBoundUrgentNotification : EmailSender
{
    public EmailBoundUrgentNotification(Transports transports) : base(transports)
    {
    }

    public DeliveryResult Send(Recipient to, Message m)
    {
        var ok = Deliver(to.Email, m.Subject, m.Body);
        var attempts = 1;
        if (!ok)
        {
            ok = Deliver(to.Email, m.Subject, m.Body);
            attempts = 2;
        }

        return new DeliveryResult(ok, ChannelName, to.Email, m.Body, attempts);
    }
}
