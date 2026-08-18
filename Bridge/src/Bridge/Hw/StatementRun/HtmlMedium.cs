using System.Text;

namespace dev.kaldiroglu.Bridge.Hw.StatementRun;

/// <summary>A ConcreteImplementor: HTML.</summary>
public sealed class HtmlMedium : IMedium
{
    private readonly StringBuilder _out = new();

    public void Heading(int level, string text) =>
        _out.Append($"<h{level}>{text}</h{level}>\n");

    public void Field(string label, string value) =>
        _out.Append($"<p><b>{label}:</b> {value}</p>\n");

    public void Row(params string[] cells)
    {
        _out.Append("<tr>");
        foreach (var cell in cells)
        {
            _out.Append($"<td>{cell}</td>");
        }

        _out.Append("</tr>\n");
    }

    public void Total(string label, string amount) =>
        _out.Append($"<p class=\"total\">{label}: {amount}</p>\n");

    public string Output() => _out.ToString();
}
