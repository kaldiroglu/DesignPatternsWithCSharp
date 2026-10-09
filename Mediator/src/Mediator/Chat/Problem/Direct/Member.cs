namespace dev.kaldiroglu.Mediator.Chat.Problem.Direct;

/// <summary>
/// Stage one: every member holds a reference to every other member.
/// <para>
/// Messages go straight from sender to receiver, and a private message reaches only its
/// receiver. But each member keeps its own list of the others. Four members hold twelve
/// references between them, and a fifth member must be added to four lists — a member who
/// was not told about the new one never sends to them.
/// </para>
/// </summary>
public sealed class Member
{
    private readonly string name;
    private readonly List<Member> others = [];
    private readonly List<string> inbox = [];

    public Member(string name)
    {
        this.name = name;
    }

    /// <summary>Both members add each other.</summary>
    public void Meet(Member other)
    {
        others.Add(other);
        other.others.Add(this);
    }

    public void Say(string text)
    {
        foreach (Member other in others)
        {
            other.Receive(name, text);
        }
    }

    public void Whisper(Member to, string text)
    {
        to.Receive(name + " (private)", text);
    }

    internal void Receive(string from, string text)
    {
        inbox.Add(from + ": " + text);
    }

    /// <summary>How many other members this member holds a reference to.</summary>
    public int References => others.Count;

    /// <summary>A copy of the inbox, as Java's <c>List.copyOf</c> gives.</summary>
    public IReadOnlyList<string> Inbox => inbox.ToList();
}
