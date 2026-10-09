namespace dev.kaldiroglu.Mediator.Chat.Solution;

/// <summary>
/// A <b>ConcreteColleague</b> written later: a guest. Like the guest client of stage three,
/// it shows whatever it receives and checks nothing — but here it receives only what the
/// room decided to send it.
/// </summary>
public sealed class Guest : IParticipant
{
    private readonly List<string> shown = [];

    public Guest(string name, ChatRoom room)
    {
        Name = name;
        room.Join(this);
    }

    public string Name { get; }

    public void Receive(string from, string text)
    {
        shown.Add(from + ": " + text);
    }

    /// <summary>A copy of what the guest sees, as Java's <c>List.copyOf</c> gives.</summary>
    public IReadOnlyList<string> Shown => shown.ToList();
}
