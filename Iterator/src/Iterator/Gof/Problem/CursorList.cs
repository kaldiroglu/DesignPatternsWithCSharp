namespace dev.kaldiroglu.Iterator.Gof.Problem;

/// <summary>
/// A list that walks itself: the position of the walk is stored in the list.
/// </summary>
/// <remarks>
/// <para>
/// This is the design GoF's motivation argues against (p. 257). The four traversal
/// operations — <c>First</c>, <c>Next</c>, <c>IsDone</c>, <c>CurrentItem</c> — are on the
/// list, and so is the cursor they move. It works for one loop. What it costs:
/// </para>
/// <list type="bullet">
///   <item>Only one walk at a time. A loop inside a loop moves the same cursor, so the outer
///     loop ends after its first item.</item>
///   <item>A second order — backwards, or every second item — means more operations and a
///     second cursor on the list itself.</item>
/// </list>
/// </remarks>
public sealed class CursorList<T>
{
    private T[] _items = new T[4];
    private int _count;
    private int _cursor;                      // the walk's position, stored in the list

    public void Append(T item)
    {
        if (_count == _items.Length)
        {
            Array.Resize(ref _items, _count * 2);
        }

        _items[_count++] = item;
    }

    public int Count => _count;

    public void First() => _cursor = 0;

    public void Next() => _cursor++;

    public bool IsDone() => _cursor >= _count;

    public T CurrentItem() => _items[_cursor];
}
