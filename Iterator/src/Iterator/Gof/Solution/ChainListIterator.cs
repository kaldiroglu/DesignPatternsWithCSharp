namespace dev.kaldiroglu.Iterator.Gof.Solution;

/// <summary>
/// A <b>ConcreteIterator</b> for <see cref="ChainList{T}"/>: it follows the links.
/// </summary>
/// <remarks>
/// It reads the list's nodes, which no client can see. GoF implementation issue 6
/// (iterators may have privileged access): an iterator is part of its list's design, so it
/// may know things the public interface does not show. Here that is <c>internal</c> access,
/// the closest C# has to Java's package access.
/// </remarks>
public sealed class ChainListIterator<T>(ChainList<T> list) : IIterator<T>
{
    private ChainList<T>.Node? _current;

    public void First() => _current = list.Head;

    public void Next() => _current = _current?.Next;

    public bool IsDone() => _current is null;

    public T CurrentItem()
    {
        if (_current is null)
        {
            throw new InvalidOperationException("the walk is over");
        }

        return _current.Item;
    }
}
