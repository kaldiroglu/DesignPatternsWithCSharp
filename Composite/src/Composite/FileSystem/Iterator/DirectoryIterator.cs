namespace dev.kaldiroglu.Composite.FileSystem.Iterator;

/// <summary>
/// Walks a directory tree depth-first.
/// </summary>
/// <remarks>
/// <para>
/// Depth-first over the whole subtree, not just the immediate children. That is the only
/// version worth having over a Composite: a nested directory is not one item, it is
/// everything underneath it, and the reason to have an iterator at all is to reach the whole
/// tree without the caller writing the recursion.
/// </para>
/// <para>
/// GoF mention exactly this under Composite's implementation notes: enumerating children is a
/// job for an Iterator, and traversal is where the two patterns meet.
/// </para>
/// </remarks>
public class DirectoryIterator : IStorageIterator
{
    private readonly Stack<IStorage> _pending = new();
    private IStorage? _current;

    public DirectoryIterator(Directory root) => PushAll(root.Elements());

    public IStorage Current =>
        _current ?? throw new InvalidOperationException("the walk has not started");

    object System.Collections.IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if (_pending.Count == 0)
        {
            _current = null;
            return false;
        }

        _current = _pending.Pop();
        if (_current is Directory directory)
        {
            PushAll(directory.Elements());
        }

        return true;
    }

    public void Reset() =>
        throw new NotSupportedException("a depth-first walk of a live tree is not restartable");

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    /// <summary>Pushed in reverse so siblings come back in the order they were added.</summary>
    private void PushAll(IReadOnlyList<IStorage> elements)
    {
        for (var i = elements.Count - 1; i >= 0; i--)
        {
            _pending.Push(elements[i]);
        }
    }
}
