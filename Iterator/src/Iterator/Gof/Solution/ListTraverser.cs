namespace dev.kaldiroglu.Iterator.Gof.Solution;

/// <summary>
/// An <b>internal iterator</b>: GoF's <c>ListTraverser</c>.
/// </summary>
/// <remarks>
/// The traverser runs the loop, and a subclass says what to do with each item in
/// <see cref="ProcessItem"/>. Returning <c>false</c> stops the walk early. The client writes
/// no loop at all — but it also cannot walk two lists side by side, because each traverser
/// runs its own loop to the end. That is GoF implementation issue 1 (who controls the
/// iteration?).
/// </remarks>
public abstract class ListTraverser<T>
{
    private readonly IIterator<T> _iterator;

    protected ListTraverser(AbstractList<T> list)
    {
        _iterator = list.CreateIterator();
    }

    /// <summary>Answers true if every item was processed, false if the walk stopped early.</summary>
    public bool Traverse()
    {
        for (_iterator.First(); !_iterator.IsDone(); _iterator.Next())
        {
            if (!ProcessItem(_iterator.CurrentItem()))
            {
                return false;
            }
        }

        return true;
    }

    protected abstract bool ProcessItem(T item);
}
