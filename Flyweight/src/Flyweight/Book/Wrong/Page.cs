using System.Text;

namespace dev.kaldiroglu.Flyweight.Book.Wrong;

public class Page
{
    private readonly int _no;
    private readonly List<Line> _lines;

    public Page(int no, int numberOfLines)
    {
        _no = no;
        _lines = new List<Line>(numberOfLines);
    }

    public void Add(Line line) => _lines.Add(line);

    public List<Line> GetLines() => _lines;

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (Line line in _lines)
        {
            sb.Append(line);
        }

        return sb.ToString();
    }
}
