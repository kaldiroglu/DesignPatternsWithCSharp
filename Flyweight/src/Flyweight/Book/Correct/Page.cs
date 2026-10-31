using System.Text;

namespace dev.kaldiroglu.Flyweight.Book.Correct;

/// <summary>A numbered page of lines. Unshared: every page has its own content.</summary>
public class Page
{
    private readonly int _no;
    private readonly int _capacity;
    private readonly List<Line> _lines;

    internal Page(int no, int capacity)
    {
        _no = no;
        _capacity = capacity;
        _lines = new List<Line>(capacity);
    }

    public bool Add(Line line)
    {
        if (_lines.Count >= _capacity)
        {
            return false;
        }

        _lines.Add(line);
        return true;
    }

    public int No() => _no;

    public IReadOnlyList<Line> GetLines() => _lines.ToList();

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (Line line in _lines)
        {
            sb.Append(line).Append(Environment.NewLine);
        }

        return sb.ToString();
    }
}
