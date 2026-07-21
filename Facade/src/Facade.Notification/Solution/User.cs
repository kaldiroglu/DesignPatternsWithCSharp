namespace dev.kaldiroglu.Facade.Notification.Solution;

/// <summary>
/// Domain model: the recipient of notifications. The Facade inspects this to
/// decide which channels are relevant. Use an object initializer to keep demos
/// short, e.g. <c>new User("u42") { Email = "a@b.com", Phone = "+1..." }</c>.
/// </summary>
public sealed class User(string id)
{
    public string Id { get; } = id;
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? SlackChannel { get; init; }
    public string? DeviceToken { get; init; }
    public bool DoNotDisturb { get; init; }
}
