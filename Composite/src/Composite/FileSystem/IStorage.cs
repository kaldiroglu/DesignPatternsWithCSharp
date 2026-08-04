namespace dev.kaldiroglu.Composite.FileSystem;

/// <summary>
/// The <b>Component</b>: everything a file system holds can do these, file or directory.
/// </summary>
/// <remarks>
/// <para>
/// Read the operations in two groups. <b>The roll-ups</b> — <see cref="Size"/>,
/// <see cref="Count"/>, <see cref="LastModified"/>, <see cref="Largest"/>,
/// <see cref="Find"/>, <see cref="FindAll"/> and <see cref="Render"/> — each ask one element
/// a question and get an answer for the whole subtree beneath it. They are why the pattern is
/// here: a caller writes <c>root.Size()</c> and no loop, at any depth, without knowing what
/// it is holding.
/// </para>
/// <para>
/// <b>The element operations</b> — <see cref="Rename"/>, <see cref="Save"/>,
/// <see cref="Delete"/>, <see cref="Copy"/> and <see cref="Move"/> — act on one element and
/// are here because every element needs them, not because they aggregate anything.
/// </para>
/// <para>
/// Child management lives on <see cref="IStorageContainer"/>, not here — the <b>safe</b> side
/// of GoF's implementation issue 4 (Declaring the child management operations, p. 168).
/// </para>
/// </remarks>
public interface IStorage
{
    string GetName();

    void Rename(string newName);

    void Save();

    /// <summary>Detaches this element from its parent. Harmless on a root.</summary>
    void Delete();

    /// <summary>A deep copy, detached from any parent.</summary>
    IStorage Copy();

    /// <summary>Moves this element into <paramref name="target"/>, leaving its old parent.</summary>
    void Move(Directory target);

    // ---------------------------------------------------------------- roll-ups

    /// <summary>Bytes, counting the whole subtree. A <b>sum</b>.</summary>
    long Size();

    /// <summary>Elements in the whole subtree, this one included. A <b>sum</b>.</summary>
    int Count();

    /// <summary>
    /// The newest modification anywhere beneath this element, itself included. A
    /// <b>maximum</b>, which is worth noticing: aggregating is not only adding.
    /// </summary>
    DateTimeOffset LastModified();

    /// <summary>
    /// The biggest leaf in the subtree — a reduction that returns an <b>element</b> rather
    /// than a number. Null only for an empty directory.
    /// </summary>
    IStorage? Largest();

    /// <summary>The first element anywhere beneath this one with that name, itself included.</summary>
    IStorage? Find(string name);

    /// <summary>Everything in the subtree matching the test, in depth-first order.</summary>
    IReadOnlyList<IStorage> FindAll(Func<IStorage, bool> test);

    /// <summary>This element and everything under it, as text.</summary>
    string Render(string indent);
}
