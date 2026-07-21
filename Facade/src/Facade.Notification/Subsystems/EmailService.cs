namespace dev.kaldiroglu.Facade.Notification.Subsystems;

/// <summary>
/// Subsystem: sends transactional emails via SMTP. In production this wraps
/// something like SendGrid / SES / MailKit. Shared unchanged by the "before"
/// client and the "after" facade — the subsystem does not change.
/// </summary>
public sealed class EmailService
{
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string _username;
    private readonly string _password;

    public EmailService(string smtpHost, int smtpPort, string username, string password)
    {
        _smtpHost = smtpHost;
        _smtpPort = smtpPort;
        _username = username;
        _password = password;
    }

    public bool Send(string to, string subject, string body)
    {
        Console.WriteLine($"[EMAIL] To: {to} | Subject: {subject}");
        return true;
    }
}
