namespace dev.kaldiroglu.Facade.Notification.Subsystems;

/// <summary>Subsystem: sends SMS via a provider API (Twilio / Vonage).</summary>
public sealed class SmsService
{
    private readonly string _accountSid;
    private readonly string _authToken;
    private readonly string _fromNumber;

    public SmsService(string accountSid, string authToken, string fromNumber)
    {
        _accountSid = accountSid;
        _authToken = authToken;
        _fromNumber = fromNumber;
    }

    public bool Send(string toPhone, string body)
    {
        Console.WriteLine($"[SMS]   To: {toPhone} | Body: {body}");
        return true;
    }
}
