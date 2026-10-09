namespace dev.kaldiroglu.Visitor.Hw.FileTree;

/// <summary>Adds up the size of every file. It knows nothing about walking the tree.</summary>
public sealed class SizeVisitor : IEntryVisitor
{
    private long total;

    public void VisitFile(FileEntry file)
    {
        total += file.Size;
    }

    public void EnterFolder(Folder folder)
    {
    }

    public void LeaveFolder(Folder folder)
    {
    }

    public long Total => total;
}
