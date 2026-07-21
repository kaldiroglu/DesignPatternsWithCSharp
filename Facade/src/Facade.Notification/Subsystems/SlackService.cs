namespace dev.kaldiroglu.Facade.Notification.Subsystems;

/// <summary>Subsystem: posts Slack messages via an incoming webhook.</summary>
public sealed class SlackService
{
    private readonly string _webhookUrl;

    public SlackService(string webhookUrl)
    {
        _webhookUrl = webhookUrl;
    }

    public bool Send(string channel, string body)
    {
        Console.WriteLine($"[SLACK] Channel: {channel} | Body: {body}");
        return true;
    }
}
