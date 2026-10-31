using System.Text;

namespace dev.kaldiroglu.Flyweight.Book.Correct;

/// <summary>A named book of pages. Unshared, like its pages and lines.</summary>
public class Book
{
    private const string PageBreak = "--- page break ---";

    private readonly string _name;
    private readonly int _capacity;
    private readonly List<Page> _pages;

    internal Book(string name, int capacity)
    {
        _name = name;
        _capacity = capacity;
        _pages = new List<Page>(capacity);
    }

    public bool Add(Page page)
    {
        if (_pages.Count >= _capacity)
        {
            return false;
        }

        _pages.Add(page);
        return true;
    }

    public string Name() => _name;

    public IReadOnlyList<Page> GetPages() => _pages.ToList();

    /// <summary>Total character occurrences in the book, not the number of objects holding them.</summary>
    public int CharacterCount() => _pages.Sum(page => page.GetLines().Sum(line => line.Length()));

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (Page page in _pages)
        {
            sb.Append(page);
            sb.Append(PageBreak).Append(Environment.NewLine);
        }

        return sb.ToString();
    }
}
