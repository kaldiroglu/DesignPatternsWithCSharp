namespace dev.kaldiroglu.Visitor.Hw.FileTree;

/// <summary>A folder: the composite. Its <c>Accept</c> does the walking.</summary>
public sealed record Folder(string Name, IReadOnlyList<IEntry> Children) : IEntry
{
    public Folder(string name, params IEntry[] children)
        : this(name, children.ToList())
    {
    }

    public void Accept(IEntryVisitor visitor)
    {
        visitor.EnterFolder(this);
        foreach (IEntry child in Children)
        {
            child.Accept(visitor);
        }
        visitor.LeaveFolder(this);
    }
}
