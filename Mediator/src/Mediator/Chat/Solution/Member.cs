namespace dev.kaldiroglu.Mediator.Chat.Solution;

/// <summary>A <b>ConcreteColleague</b>: a team member. It talks only to the room.</summary>
public sealed class Member : IParticipant
{
    private readonly ChatRoom room;
    private readonly List<string> shown = [];

    public Member(string name, ChatRoom room)
    {
        Name = name;
        this.room = room;
        room.Join(this);
    }

    public string Name { get; }

    public void Say(string text)
    {
        room.Say(Name, text);
    }

    public void Whisper(string to, string text)
    {
        room.Whisper(Name, to, text);
    }

    public void Block(string other)
    {
        room.Block(Name, other);
    }

    public void Receive(string from, string text)
    {
        shown.Add(from + ": " + text);
    }

    /// <summary>A copy of what the member sees, as Java's <c>List.copyOf</c> gives.</summary>
    public IReadOnlyList<string> Shown => shown.ToList();
}
