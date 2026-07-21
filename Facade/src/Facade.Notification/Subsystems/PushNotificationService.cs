namespace dev.kaldiroglu.Facade.Notification.Subsystems;

/// <summary>Subsystem: sends push notifications via FCM / APNs.</summary>
public sealed class PushNotificationService
{
    private readonly string _firebaseCredentialsPath;

    public PushNotificationService(string firebaseCredentialsPath)
    {
        _firebaseCredentialsPath = firebaseCredentialsPath;
    }

    public bool Send(string deviceToken, string title, string body)
    {
        string masked = deviceToken[..Math.Min(8, deviceToken.Length)];
        Console.WriteLine($"[PUSH]  Token: {masked}*** | Title: {title}");
        return true;
    }
}
