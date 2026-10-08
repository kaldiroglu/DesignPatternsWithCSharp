namespace dev.kaldiroglu.Iterator.Gof.Solution;

/// <summary>
/// A second <b>ConcreteIterator</b> for the same <see cref="List{T}"/>: back to front.
/// </summary>
/// <remarks>GoF's <c>ReverseListIterator</c>. The list did not change to get a second order.</remarks>
public sealed class ReverseListIterator<T>(List<T> list) : IIterator<T>
{
    private int _current;

    public void First() => _current = list.Count - 1;

    public void Next() => _current--;

    public bool IsDone() => _current < 0;

    public T CurrentItem()
    {
        if (IsDone())
        {
            throw new InvalidOperationException("the walk is over");
        }

        return list.Get(_current);
    }
}
