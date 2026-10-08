namespace dev.kaldiroglu.Iterator.Gof.Solution;

/// <summary>A <b>ConcreteIterator</b> for <see cref="List{T}"/>: front to back, by index.</summary>
public sealed class ListIterator<T>(List<T> list) : IIterator<T>
{
    private int _current;

    public void First() => _current = 0;

    public void Next() => _current++;

    public bool IsDone() => _current >= list.Count;

    public T CurrentItem()
    {
        if (IsDone())
        {
            throw new InvalidOperationException("the walk is over");
        }

        return list.Get(_current);
    }
}
