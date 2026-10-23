namespace dev.kaldiroglu.Iterator;

/// <summary>
/// An <see cref="IEnumerable{T}"/> made from a function that creates an enumerator.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="IEnumerable{T}"/> is not a delegate type, so an enumerable cannot be written
/// as a lambda such as <c>() =&gt; new LevelOrderIterator(this)</c>. This class does that
/// job: it holds the lambda and calls
/// it each time a loop asks for an enumerator.
/// </para>
/// <para>
/// It is not part of the pattern. It is only here so that <c>Department.ByLevel()</c> and
/// <c>PartIterator.PartsOf()</c> can each return a new iterator from one line.
/// </para>
/// </remarks>
internal sealed class LambdaEnumerable<T>(Func<IEnumerator<T>> create) : IEnumerable<T>
{
    public IEnumerator<T> GetEnumerator() => create();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}
