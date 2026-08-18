using System.Text;

namespace dev.kaldiroglu.Bridge.Hw.StatementRun;

/// <summary>A ConcreteImplementor: fixed-width text, for the body of an email.</summary>
public sealed class PlainTextMedium : IMedium
{
    private readonly StringBuilder _out = new();

    public void Heading(int level, string text)
    {
        var underline = level == 1 ? "=" : "-";
        _out.Append(text).Append('\n')
            .Append(string.Concat(Enumerable.Repeat(underline, text.Length))).Append('\n');
    }

    public void Field(string label, string value) =>
        _out.Append($"{label + ":",-18} {value}\n");

    public void Row(params string[] cells) =>
        _out.Append("  ").Append(string.Join("   ", cells)).Append('\n');

    public void Total(string label, string amount) =>
        _out.Append($"{label.ToUpperInvariant() + ":",-18} {amount}\n");

    public string Output() => _out.ToString();
}
