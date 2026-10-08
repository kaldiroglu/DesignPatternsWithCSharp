using System.Collections;

namespace dev.kaldiroglu.Iterator.Hw.Paging;

/// <summary>
/// Homework 3: walk every item of a paged result without loading every page.
/// </summary>
/// <remarks>
/// The caller sees one long sequence and writes a simple loop. Behind it, the iterator asks
/// the source for the next page only when the current one is used up. A caller that stops
/// after the first match never causes the later pages to be fetched —
/// <see cref="PagesFetched"/> shows how many were.
/// </remarks>
public sealed class PagedIterator<T>(PageSource<T> source) : IEnumerator<T>
{
    private int _pageNumber;
    private int _pagesFetched;
    private IEnumerator<T> _page = Enumerable.Empty<T>().GetEnumerator();
    private bool _finished;
    private bool _hasCurrent;

    public T Current =>
        _hasCurrent ? _page.Current : throw new InvalidOperationException("no current item: call MoveNext first");

    object? IEnumerator.Current => Current;

    public bool MoveNext()
    {
        while (!_page.MoveNext())
        {
            if (_finished)
            {
                _hasCurrent = false;
                return false;
            }

            IReadOnlyList<T> page = source(_pageNumber++);
            _pagesFetched++;
            if (page.Count == 0)
            {
                _finished = true;
            }

            _page.Dispose();
            _page = page.GetEnumerator();
        }

        _hasCurrent = true;
        return true;
    }

    /// <summary>How many pages have been asked for so far.</summary>
    public int PagesFetched => _pagesFetched;

    /// <summary>Not supported: starting again would fetch the pages again. Create a new iterator.</summary>
    public void Reset() =>
        throw new NotSupportedException("a paged walk cannot start again; create a new iterator");

    public void Dispose() => _page.Dispose();
}
