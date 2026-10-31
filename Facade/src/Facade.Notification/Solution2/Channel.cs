namespace dev.kaldiroglu.Facade.Notification.Solution2;

/// <summary>
/// Channels the notification facade can dispatch to. Clients pick which subset to use; the
/// facade still owns the dispatch logic.
/// </summary>
public enum Channel
{
    Email,
    Sms,
    Slack,
    Push,
}
