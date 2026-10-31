using System.Text;

namespace dev.kaldiroglu.Flyweight.Book.Wrong;

public class Book
{
    private static readonly Character EndOfPage = new(12);

    private readonly string _bookName;
    private readonly List<Page> _pages;

    public Book(string bookName, int numberOfPages)
    {
        _bookName = bookName;
        _pages = new List<Page>(numberOfPages);
    }

    public void Add(Page page) => _pages.Add(page);

    public List<Page> GetPages() => _pages;

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (Page page in _pages)
        {
            sb.Append(page);
            sb.Append(EndOfPage);
        }

        return sb.ToString();
    }
}
