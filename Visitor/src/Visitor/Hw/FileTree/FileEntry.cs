namespace dev.kaldiroglu.Visitor.Hw.FileTree;

public sealed record FileEntry(string Name, long Size) : IEntry
{
    public void Accept(IEntryVisitor visitor) => visitor.VisitFile(this);
}
