using dev.kaldiroglu.Bridge.Notifications.Domain;

namespace dev.kaldiroglu.Bridge.Notifications.Solution.Classic;

/// <summary>
/// The <b>Implementor</b>: what every delivery channel can do, expressed as primitives.
/// <para>
/// Everything here is a question about the <i>channel</i>, and nothing here is a question about
/// notifications. There is no <c>SendUrgent</c>, no <c>SendDigest</c> — those are the
/// abstraction's business, and if they appeared here the two hierarchies would have grown back
/// together.
/// </para>
/// <para>
/// GoF, p. 153: "The Implementor interface provides only primitive operations, and Abstraction
/// defines higher-level operations based on these primitives."
/// </para>
/// </summary>
public interface INotificationChannel
{
    /// <summary>What this channel is called in logs and results.</summary>
    string Name { get; }

    /// <summary>How this channel identifies a person. An email address, a phone number, a token.</summary>
    string AddressOf(Recipient recipient);

    /// <summary>The longest body this channel will carry. The abstraction asks; it never assumes.</summary>
    int MaxBodyLength { get; }

    /// <summary>Whether the channel can carry a separate subject line, or only one blob of text.</summary>
    bool SupportsSubject { get; }

    /// <summary>Hand one message to the channel. Returns false if it could not be delivered.</summary>
    bool Deliver(string address, string subject, string body);
}
