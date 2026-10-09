using System.Globalization;

namespace dev.kaldiroglu.Visitor.Hw.FileTree;

/// <summary>Lists the tree with indentation. It uses enter and leave to know the depth.</summary>
public sealed class ListingVisitor : IEntryVisitor
{
    private readonly List<string> lines = new();
    private int depth;

    public void VisitFile(FileEntry file)
    {
        lines.Add(Indent() + file.Name + " (" + file.Size.ToString(CultureInfo.InvariantCulture) + ")");
    }

    public void EnterFolder(Folder folder)
    {
        lines.Add(Indent() + folder.Name + "/");
        depth++;
    }

    public void LeaveFolder(Folder folder)
    {
        depth--;
    }

    public IReadOnlyList<string> Lines => lines.ToList();

    /// <summary>Two spaces for each level, as Java's <c>"  ".repeat(depth)</c>.</summary>
    private string Indent() => new(' ', 2 * depth);
}
