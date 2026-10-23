namespace dev.kaldiroglu.Visitor.Hw.FileTree;

/// <summary>
/// The <b>Visitor</b> for a folder tree. A folder is visited twice — when the walk enters it
/// and when it leaves — so a visitor can keep track of the depth.
/// </summary>
public interface IEntryVisitor
{
    void VisitFile(FileEntry file);

    void EnterFolder(Folder folder);

    void LeaveFolder(Folder folder);
}
