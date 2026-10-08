using System.Collections;

namespace dev.kaldiroglu.Iterator.FileSystem;

/// <summary>
/// The <b>ConcreteIterator</b>: walks the elements of one directory.
/// </summary>
/// <remarks>
/// <para>
/// It walks only the directory's own elements. A folder inside it is one element; its
/// contents are not visited. The Composite port's <c>FileSystem.Iterator.DirectoryIterator</c>
/// walks the whole tree.
/// </para>
/// <para>
/// <typeparamref name="Storage"/> is a type parameter, not the <see cref="IStorage"/>
/// interface. The name is kept from the Java original on purpose, because one of the deck's
/// exercises asks about it. In Java the type parameter hides the interface of the same name.
/// In C# the interface is called <c>IStorage</c>, so nothing is hidden, but the name still
/// reads like a type and not like a parameter. C# naming would call it <c>TStorage</c>.
/// </para>
/// </remarks>
public class DirectoryIterator<Storage> : IEnumerator<Storage>
{
    private readonly Directory _dir;
    private IReadOnlyList<Storage> _elements;
    private IEnumerator<Storage> _iterator;

    public DirectoryIterator(Directory dir)
    {
        _dir = dir;
        // The same unchecked cast as the Java original. It succeeds only when Storage is IStorage.
        _elements = (IReadOnlyList<Storage>)(object)dir.Elements;
        _iterator = _elements.GetEnumerator();
    }

    public bool MoveNext() => _iterator.MoveNext();

    public Storage Current => _iterator.Current;

    object? IEnumerator.Current => Current;

    /// <summary>Starts the walk again, from the directory's current elements.</summary>
    public void Reset()
    {
        _iterator.Dispose();
        _elements = (IReadOnlyList<Storage>)(object)_dir.Elements;
        _iterator = _elements.GetEnumerator();
    }

    public void Dispose()
    {
        _iterator.Dispose();
        GC.SuppressFinalize(this);
    }
}
