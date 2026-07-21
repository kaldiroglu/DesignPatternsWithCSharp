namespace dev.kaldiroglu.Facade.Notification.Subsystems;

/// <summary>Subsystem: writes an audit record for every notification dispatch.</summary>
public sealed class NotificationLogger
{
    private readonly string _logFilePath;

    public NotificationLogger(string logFilePath)
    {
        _logFilePath = logFilePath;
    }

    public void Record(string userId, string channel, bool success)
    {
        Console.WriteLine($"[LOG]   {DateTimeOffset.UtcNow:O} | user={userId} channel={channel} success={success}");
    }
}
