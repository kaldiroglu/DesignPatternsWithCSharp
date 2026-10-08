namespace dev.kaldiroglu.Iterator.Hw.Paging;

/// <summary>
/// Where the pages come from: a web service, a database, a file. One call returns one page.
/// An empty page means there is nothing more.
/// </summary>
/// <remarks>
/// Java declares this as a <c>@FunctionalInterface</c>. The C# form of a one-method interface
/// that callers fill with a lambda is a delegate, so it is a delegate here.
/// </remarks>
public delegate IReadOnlyList<T> PageSource<T>(int number);
