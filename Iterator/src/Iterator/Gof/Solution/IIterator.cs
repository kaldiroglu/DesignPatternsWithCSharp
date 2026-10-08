namespace dev.kaldiroglu.Iterator.Gof.Solution;

/// <summary>
/// The <b>Iterator</b>, with GoF's four operations.
/// </summary>
/// <remarks>
/// <para>
/// The position of the walk lives in the iterator, not in the list. So any number of
/// iterators can walk the same list at the same time.
/// </para>
/// <para>
/// .NET's <see cref="IEnumerator{T}"/> has the same job with two main members:
/// <c>MoveNext()</c> is <c>Next()</c> followed by <c>!IsDone()</c>, and <c>Current</c> is
/// <c>CurrentItem()</c>. The first <c>MoveNext()</c> plays the part of <c>First()</c>.
/// Starting again means asking the list for a new iterator; <c>Reset()</c> exists but many
/// enumerators do not support it.
/// </para>
/// </remarks>
public interface IIterator<T>
{
    void First();

    void Next();

    bool IsDone();

    T CurrentItem();
}
