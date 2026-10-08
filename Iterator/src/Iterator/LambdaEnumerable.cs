namespace dev.kaldiroglu.Iterator;

/// <summary>
/// An <see cref="IEnumerable{T}"/> made from a function that creates an enumerator.
/// </summary>
/// <remarks>
/// <para>
/// Java's <c>Iterable</c> has one method, so Java can write an iterable as a lambda:
/// <c>() -&gt; new LevelOrderIterator(this)</c>. <see cref="IEnumerable{T}"/> is not a
/// delegate type, so C# cannot. This class does the same job: it holds the lambda and calls
/// it each time a loop asks for an enumerator.
/// </para>
/// <para>
/// It is not part of the pattern. It is only here so that <c>Department.ByLevel()</c> and
/// <c>PartIterator.PartsOf()</c> keep the shape of the Java code.
/// </para>
/// </remarks>
internal sealed class LambdaEnumerable<T>(Func<IEnumerator<T>> create) : IEnumerable<T>
{
    public IEnumerator<T> GetEnumerator() => create();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}
