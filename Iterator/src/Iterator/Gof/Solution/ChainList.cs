namespace dev.kaldiroglu.Iterator.Gof.Solution;

/// <summary>
/// A second <b>ConcreteAggregate</b>, stored as a chain of nodes.
/// </summary>
/// <remarks>
/// GoF use a skip list here. Any structure that is not an array makes the same point: it
/// cannot be walked by index cheaply, so it creates its own kind of iterator, one that
/// follows the links. A client that walks an <see cref="AbstractList{T}"/> does not see the
/// difference.
/// </remarks>
public sealed class ChainList<T> : AbstractList<T>
{
    internal sealed class Node(T item)
    {
        internal T Item { get; } = item;

        internal Node? Next { get; set; }
    }

    private Node? _head;
    private Node? _tail;
    private int _count;

    public override void Append(T item)
    {
        var node = new Node(item);
        if (_head is null)
        {
            _head = node;
        }
        else
        {
            _tail!.Next = node;
        }

        _tail = node;
        _count++;
    }

    public override int Count => _count;

    public override IIterator<T> CreateIterator() => new ChainListIterator<T>(this);

    internal Node? Head => _head;
}
