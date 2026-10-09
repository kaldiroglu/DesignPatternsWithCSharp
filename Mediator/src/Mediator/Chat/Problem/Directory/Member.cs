namespace dev.kaldiroglu.Mediator.Chat.Problem.Directory;

/// <summary>
/// Stage two: members find each other through a shared <see cref="Directory"/>.
/// <para>
/// A new member is added in one place. But every member still sends to the others itself,
/// so every sender must apply the rules: skip itself, skip anyone who has blocked it. The
/// rules are in the sending code of every kind of member.
/// </para>
/// </summary>
public sealed class Member
{
    private readonly Directory directory;
    private readonly HashSet<string> blocked = [];
    private readonly List<string> inbox = [];

    public Member(string name, Directory directory)
    {
        Name = name;
        this.directory = directory;
        directory.Add(this);
    }

    public string Name { get; }

    public void Block(string other)
    {
        blocked.Add(other);
    }

    public void Say(string text)
    {
        foreach (Member other in directory.Members)
        {
            if (other != this && !other.blocked.Contains(Name))     // the rules, in the sender
            {
                other.Receive(Name, text);
            }
        }
    }

    public void Whisper(string to, string text)
    {
        Member? other = directory.Find(to);
        if (other != null && !other.blocked.Contains(Name))
        {
            other.Receive(Name + " (private)", text);
        }
    }

    internal void Receive(string from, string text)
    {
        inbox.Add(from + ": " + text);
    }

    /// <summary>A copy of the inbox, as Java's <c>List.copyOf</c> gives.</summary>
    public IReadOnlyList<string> Inbox => inbox.ToList();
}
