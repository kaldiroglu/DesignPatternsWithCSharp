using System.Text;

namespace dev.kaldiroglu.Bridge.Hw.StatementRun;

/// <summary>
/// A ConcreteImplementor: what a screen reader will say.
/// <para>
/// This is the medium that decides the shape of <see cref="IMedium"/>. It has no page, no column
/// and no font, so any primitive that mentioned one would have to be faked here — and a faked
/// primitive is the beginning of the end of a Bridge. Because the interface asks only about
/// meaning, this class is as short as the other two.
/// </para>
/// </summary>
public sealed class SpokenMedium : IMedium
{
    private readonly StringBuilder _out = new();

    public void Heading(int level, string text) =>
        _out.Append(level == 1 ? "Document: " : "Section: ").Append(text).Append(". ");

    public void Field(string label, string value) =>
        _out.Append(label).Append(", ").Append(value).Append(". ");

    public void Row(params string[] cells) =>
        _out.Append("Line: ").Append(string.Join(", ", cells)).Append(". ");

    public void Total(string label, string amount) =>
        _out.Append(label).Append(" of ").Append(amount).Append(". ");

    public string Output() => _out.ToString().Trim();
}
