using System.Text;

namespace dev.kaldiroglu.Command.Gof;

/// <summary>
/// GoF's <c>Document</c>: a <b>Receiver</b>. It knows how to open, copy and paste, and it
/// knows nothing about menus.
/// <para>
/// Design Patterns, p. 233: "Document objects... are receivers of commands like
/// PasteCommand".
/// </para>
/// </summary>
public sealed class Document
{
    private readonly Clipboard _clipboard;
    private readonly StringBuilder _text = new();

    public Document(string name, Clipboard clipboard)
    {
        Name = name;
        _clipboard = clipboard;
    }

    public string Name { get; }

    public bool IsOpen { get; private set; }

    public void Open() => IsOpen = true;

    public void Type(string words) => _text.Append(words);

    public void Copy() => _clipboard.Put(_text.ToString());

    public void Paste() => _text.Append(_clipboard.Contents);

    public string Text => _text.ToString();
}
