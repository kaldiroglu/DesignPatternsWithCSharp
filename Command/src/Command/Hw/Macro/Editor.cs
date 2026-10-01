using System.Text;

namespace dev.kaldiroglu.Command.Hw.Macro;

/// <summary>The <b>Receiver</b>: a line of text with the cursor at its end.</summary>
public sealed class Editor
{
    private readonly StringBuilder _text = new();

    public void Type(string words) => _text.Append(words);

    public void DeleteLast(int count) => _text.Length = Math.Max(0, _text.Length - count);

    public void UpperCaseLastWord()
    {
        var current = _text.ToString();
        var start = current.LastIndexOf(' ') + 1;
        var word = current[start..].ToUpperInvariant();
        _text.Remove(start, _text.Length - start).Append(word);
    }

    public string Text => _text.ToString();
}
