namespace dev.kaldiroglu.Mediator.Chat.Problem.Directory;

/// <summary>Stage two: one shared list of members. A new member is added once, here.</summary>
/// <remarks>
/// Inside this namespace the name <c>Directory</c> means this class, not
/// <c>System.IO.Directory</c>: a type in the current namespace is found before any type that
/// a <c>using</c> brings in.
/// </remarks>
public sealed class Directory
{
    private readonly List<Member> members = [];

    public void Add(Member member)
    {
        members.Add(member);
    }

    /// <summary>A copy of the members, as Java's <c>List.copyOf</c> gives.</summary>
    public IReadOnlyList<Member> Members => members.ToList();

    /// <summary>The member with this name, or <c>null</c> when there is none.</summary>
    public Member? Find(string name)
    {
        return members.FirstOrDefault(m => m.Name == name);
    }
}
