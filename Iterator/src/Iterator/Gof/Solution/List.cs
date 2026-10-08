namespace dev.kaldiroglu.Iterator.Gof.Solution;

/// <summary>
/// A <b>ConcreteAggregate</b> stored in an array: GoF's <c>List</c>.
/// </summary>
/// <remarks>
/// <para>It has no traversal operations and no cursor. Compare <c>Problem.CursorList</c>.</para>
/// <para>
/// The name is the same as .NET's <see cref="System.Collections.Generic.List{T}"/>. Inside
/// this namespace, <c>List&lt;T&gt;</c> means this class, because a type in the current
/// namespace wins over a type brought in by a <c>using</c>. Code in this namespace that needs
/// .NET's list writes <c>System.Collections.Generic.List&lt;T&gt;</c> in full, as the Java
/// code writes <c>java.util.List</c>.
/// </para>
/// </remarks>
public sealed class List<T> : AbstractList<T>
{
    private T[] _items = new T[4];
    private int _count;

    public override void Append(T item)
    {
        if (_count == _items.Length)
        {
            Array.Resize(ref _items, _count * 2);
        }

        _items[_count++] = item;
    }

    public override int Count => _count;

    public T Get(int index)
    {
        if (index < 0 || index >= _count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, "no item at this index");
        }

        return _items[index];
    }

    public override IIterator<T> CreateIterator() => new ListIterator<T>(this);
}
