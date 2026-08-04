using dev.kaldiroglu.Composite.FileSystem.Iterator;

namespace dev.kaldiroglu.Composite.FileSystem;

/// <summary>
/// Child management, kept off <see cref="IStorage"/> on purpose — GoF implementation issue 4
/// (Declaring the child management operations, p. 168).
/// </summary>
/// <remarks>
/// Declaring <c>Add</c> here rather than on the Component means a <see cref="File"/> cannot be
/// given children: the method does not exist on it, so the mistake is a compile error rather
/// than a run-time one. The price is that code building a tree has to know it is holding a
/// <see cref="Directory"/>.
/// </remarks>
public interface IStorageContainer
{
    void Add(IStorage element);

    void Remove(IStorage element);

    IReadOnlyList<IStorage> Elements();

    /// <summary>Depth-first over the whole subtree, not just the immediate children.</summary>
    IStorageIterator Iterator();
}
