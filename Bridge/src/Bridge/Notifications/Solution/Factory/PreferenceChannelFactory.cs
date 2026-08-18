using dev.kaldiroglu.Bridge.Notifications.Domain;
using dev.kaldiroglu.Bridge.Notifications.Solution.Classic;

namespace dev.kaldiroglu.Bridge.Notifications.Solution.Factory;

/// <summary>
/// Picks the channel from what the user asked for — a value that lives in a database and is not
/// known until the program runs.
/// <para>
/// This is the whole argument for Bridge over inheritance, reduced to one method. Under any
/// design where the channel is a base class, this method cannot exist: you cannot choose a base
/// class at run time.
/// </para>
/// <para>
/// The channels are built once and shared, which is GoF's implementation issue 3 in its
/// simplest form — see <c>Solution.Shared</c> for what that costs when a channel holds state.
/// </para>
/// </summary>
public sealed class PreferenceChannelFactory : IChannelFactory
{
    private readonly Dictionary<ChannelKind, INotificationChannel> _channels;

    public PreferenceChannelFactory(Transports transports) =>
        _channels = new Dictionary<ChannelKind, INotificationChannel>
        {
            [ChannelKind.Email] = new EmailChannel(transports),
            [ChannelKind.Sms] = new SmsChannel(transports),
            [ChannelKind.Push] = new PushChannel(transports)
        };

    public INotificationChannel ChannelFor(Recipient recipient) => _channels[recipient.Preferred];

    /// <summary>
    /// Registering a fourth channel is one line, and no notification kind is touched.
    /// </summary>
    public PreferenceChannelFactory Register(ChannelKind kind, INotificationChannel channel)
    {
        _channels[kind] = channel;
        return this;
    }
}
