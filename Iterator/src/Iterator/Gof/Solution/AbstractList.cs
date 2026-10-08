namespace dev.kaldiroglu.Iterator.Gof.Solution;

/// <summary>
/// The <b>Aggregate</b>: GoF's <c>AbstractList</c>.
/// </summary>
/// <remarks>
/// <see cref="CreateIterator"/> is a factory method. Each kind of list creates the iterator
/// that knows its own structure, so a client that holds an <c>AbstractList</c> can walk an
/// array or a chain of nodes with the same code. GoF call this polymorphic iteration.
/// </remarks>
public abstract class AbstractList<T>
{
    public abstract IIterator<T> CreateIterator();

    public abstract int Count { get; }

    public abstract void Append(T item);
}
