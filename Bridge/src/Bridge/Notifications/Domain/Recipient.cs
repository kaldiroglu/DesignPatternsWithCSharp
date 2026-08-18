namespace dev.kaldiroglu.Bridge.Notifications.Domain;

/// <summary>
/// Somebody we need to reach, and the ways we are allowed to reach them.
/// <para>
/// The <c>Preferred</c> field is where the pressure comes from: it is chosen by the user,
/// stored in a database, and known only while the program is running.
/// </para>
/// </summary>
public record Recipient(string Name, string Email, string Phone, string DeviceToken,
    ChannelKind Preferred)
{
    public static Recipient Of(string name, ChannelKind preferred)
    {
        var handle = name.ToLowerInvariant();
        return new Recipient(name, $"{handle}@example.com", "+90-555-0100",
            $"device-{handle}", preferred);
    }
}
