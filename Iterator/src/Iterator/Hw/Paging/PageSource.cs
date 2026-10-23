namespace dev.kaldiroglu.Iterator.Hw.Paging;

/// <summary>
/// Where the pages come from: a web service, a database, a file. One call returns one page.
/// An empty page means there is nothing more.
/// </summary>
/// <remarks>
/// Callers fill it with a lambda, so it is a delegate.
/// </remarks>
public delegate IReadOnlyList<T> PageSource<T>(int number);
