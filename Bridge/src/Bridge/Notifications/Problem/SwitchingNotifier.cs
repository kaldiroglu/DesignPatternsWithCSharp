using dev.kaldiroglu.Bridge.Notifications.Domain;

namespace dev.kaldiroglu.Bridge.Notifications.Problem;

/// <summary>
/// Naive design 1: one class, one method, and a switch on each axis.
/// <para>
/// This is what the code looks like after the second channel arrives and before anybody has
/// time to think. It works, and for two kinds and two channels it is arguably the clearest
/// thing in the file.
/// </para>
/// <para>
/// <b>Every branch is a pair.</b> Kinds x channels, written out by hand. Three kinds and three
/// channels is nine branches in one method — and the ninth is written by somebody who has
/// forgotten what the first one does.
/// </para>
/// <para>
/// <b>Both axes are frozen together.</b> Adding a channel means editing every kind's branch;
/// adding a kind means editing every channel's. There is no edit that touches one axis alone.
/// </para>
/// <para>
/// <b>The rules leak.</b> Look at how many times <c>SmsLimit</c> appears below. The SMS length
/// rule is not owned by anything; it is repeated wherever somebody remembered it — and, in
/// <c>SendDigest</c>, forgotten.
/// </para>
/// </summary>
public sealed class SwitchingNotifier
{
    /// <summary>What kind of notification this is. The second axis of the problem.</summary>
    public enum Kind
    {
        Simple,
        Urgent,
        Digest
    }

    private readonly Transports _transports;

    public SwitchingNotifier(Transports transports) => _transports = transports;

    public DeliveryResult Send(Kind kind, ChannelKind channel, Recipient to, Message message) =>
        kind switch
        {
            Kind.Simple => SendSimple(channel, to, message),
            Kind.Urgent => SendUrgent(channel, to, message),
            Kind.Digest => SendDigest(channel, to, message),
            _ => throw new ArgumentOutOfRangeException(nameof(kind))
        };

    private DeliveryResult SendSimple(ChannelKind channel, Recipient to, Message m)
    {
        switch (channel)
        {
            case ChannelKind.Email:
            {
                var ok = _transports.SmtpSend(to.Email, m.Subject, m.Body);
                return new DeliveryResult(ok, "email", to.Email, m.Body, 1);
            }
            case ChannelKind.Sms:
            {
                var text = Clip($"{m.Subject}: {m.Body}", Transports.SmsLimit);
                var ok = _transports.SmsSubmit(to.Phone, text);
                return new DeliveryResult(ok, "sms", to.Phone, text, 1);
            }
            case ChannelKind.Push:
            {
                var payload = Clip(m.Subject, Transports.PushLimit);
                var ok = _transports.PushNotify(to.DeviceToken, payload);
                return new DeliveryResult(ok, "push", to.DeviceToken, payload, 1);
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(channel));
        }
    }

    private DeliveryResult SendUrgent(ChannelKind channel, Recipient to, Message m)
    {
        // "Urgent" means: try again once if it fails. The retry has to be written out
        // separately for every channel, because there is nothing common to hang it on.
        switch (channel)
        {
            case ChannelKind.Email:
            {
                var ok = _transports.SmtpSend(to.Email, m.Subject, m.Body);
                var attempts = 1;
                if (!ok)
                {
                    ok = _transports.SmtpSend(to.Email, m.Subject, m.Body);
                    attempts = 2;
                }

                return new DeliveryResult(ok, "email", to.Email, m.Body, attempts);
            }
            case ChannelKind.Sms:
            {
                var text = Clip($"URGENT {m.Subject}: {m.Body}", Transports.SmsLimit);
                var ok = _transports.SmsSubmit(to.Phone, text);
                var attempts = 1;
                if (!ok)
                {
                    ok = _transports.SmsSubmit(to.Phone, text);
                    attempts = 2;
                }

                return new DeliveryResult(ok, "sms", to.Phone, text, attempts);
            }
            case ChannelKind.Push:
            {
                var payload = Clip($"URGENT {m.Subject}", Transports.PushLimit);
                var ok = _transports.PushNotify(to.DeviceToken, payload);
                var attempts = 1;
                if (!ok)
                {
                    ok = _transports.PushNotify(to.DeviceToken, payload);
                    attempts = 2;
                }

                return new DeliveryResult(ok, "push", to.DeviceToken, payload, attempts);
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(channel));
        }
    }

    private DeliveryResult SendDigest(ChannelKind channel, Recipient to, Message m)
    {
        var combined = $"Digest: {m.Body}";
        switch (channel)
        {
            case ChannelKind.Email:
            {
                var ok = _transports.SmtpSend(to.Email, "Your digest", combined);
                return new DeliveryResult(ok, "email", to.Email, combined, 1);
            }
            case ChannelKind.Sms:
            {
                // The 160-character rule is missing here. Nobody removed it; the person who
                // added digests simply did not know it existed. The transport throws.
                var ok = _transports.SmsSubmit(to.Phone, combined);
                return new DeliveryResult(ok, "sms", to.Phone, combined, 1);
            }
            case ChannelKind.Push:
            {
                var payload = Clip(combined, Transports.PushLimit);
                var ok = _transports.PushNotify(to.DeviceToken, payload);
                return new DeliveryResult(ok, "push", to.DeviceToken, payload, 1);
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(channel));
        }
    }

    private static string Clip(string text, int limit) =>
        text.Length <= limit ? text : text[..limit];
}
