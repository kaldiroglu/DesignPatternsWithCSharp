namespace dev.kaldiroglu.Observer.Hw.Inbox;

/// <summary>
/// Homework 3: an inbox shown in several places — a badge with the unread count, a list of
/// subjects, and a desktop notification.
/// <para>
/// The homework question: should the inbox push the new message, or only say "something
/// changed" and let each view pull what it needs? Here it pushes the message, because every
/// view needs it and the message is small. The badge pulls the count, because the count is
/// not in the message. Both styles in one subject: GoF implementation issue 6 (avoiding
/// observer-specific update protocols: the push and pull models).
/// </para>
/// </summary>
public sealed class Inbox
{
    public sealed record Message(string From, string Subject);

    private readonly List<Message> messages = [];
    private int unread;
    private readonly List<Action<Message>> views = [];

    /// <summary>
    /// Any function that takes a message can be a view: the observer is a lambda. Java uses
    /// <c>Consumer&lt;Message&gt;</c>; the C# delegate for the same job is
    /// <see cref="Action{T}"/>.
    /// </summary>
    public void OnNewMessage(Action<Message> view)
    {
        views.Add(view);
    }

    public void Receive(Message message)
    {
        messages.Add(message);
        unread++;
        foreach (var view in views.ToList())
        {
            view(message);
        }
    }

    public void ReadAll()
    {
        unread = 0;
    }

    public int Unread => unread;
}
