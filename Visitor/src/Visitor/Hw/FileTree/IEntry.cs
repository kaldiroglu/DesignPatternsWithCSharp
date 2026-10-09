namespace dev.kaldiroglu.Visitor.Hw.FileTree;

/// <summary>
/// Homework 2: who walks the tree?
/// <para>
/// Here the structure walks itself: <see cref="Folder.Accept"/> visits the folder and then
/// passes the visitor to each child. Every visitor gets the walk for free and cannot get it
/// wrong. The cost is that every visitor gets the same walk. GoF implementation issue 2 (who
/// is responsible for traversing the object structure?) lists this as the first answer.
/// </para>
/// </summary>
public interface IEntry
{
    string Name { get; }

    void Accept(IEntryVisitor visitor);
}
