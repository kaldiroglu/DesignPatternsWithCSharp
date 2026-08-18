namespace dev.kaldiroglu.Bridge.Notifications.Domain;

/// <summary>
/// The three vendor SDKs, as they were handed to us.
/// <para>
/// This is the part nobody gets to redesign. Each one has its own name for "send", its own idea
/// of an address, and its own limits. They have nothing in common but the fact that a message
/// comes out the other end.
/// </para>
/// <para>
/// Both the naive designs and the Bridge design end up here. That is deliberate: the two are
/// then doing identical work on identical infrastructure, and every remaining difference is a
/// difference of design.
/// </para>
/// </summary>
public sealed class Transports
{
    /// <summary>An SMS is 160 characters. This is not negotiable and never has been.</summary>
    public const int SmsLimit = 160;

    /// <summary>Push payloads are capped by the platform.</summary>
    public const int PushLimit = 120;

    /// <summary>Mail servers will take far more than we will ever send.</summary>
    public const int EmailLimit = 10_000;

    private readonly TransportLog _log;

    public Transports(TransportLog log) => _log = log;

    /// <summary>The mail vendor's API: a subject and a body, to an address.</summary>
    public bool SmtpSend(string emailAddress, string subject, string body)
    {
        _log.OpenConnection();
        return _log.Record("email", emailAddress, body);
    }

    /// <summary>
    /// The SMS vendor's API: one string, to a number. No subject. Hard 160-character limit.
    /// </summary>
    public bool SmsSubmit(string phoneNumber, string text)
    {
        if (text.Length > SmsLimit)
        {
            throw new ArgumentException($"SMS over {SmsLimit} characters");
        }

        _log.OpenConnection();
        return _log.Record("sms", phoneNumber, text);
    }

    /// <summary>The push vendor's API: a payload, to a device token.</summary>
    public bool PushNotify(string deviceToken, string payload)
    {
        if (payload.Length > PushLimit)
        {
            throw new ArgumentException($"push payload over {PushLimit}");
        }

        _log.OpenConnection();
        return _log.Record("push", deviceToken, payload);
    }
}
