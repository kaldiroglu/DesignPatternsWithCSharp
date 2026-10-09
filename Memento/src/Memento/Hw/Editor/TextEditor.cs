namespace dev.kaldiroglu.Memento.Hw.Editor;

/// <summary>
/// Homework 1: the <b>Originator</b>. A text and a cursor; it can save both in a memento and
/// take them back.
/// </summary>
public sealed class TextEditor
{
    /// <summary>The <b>Memento</b>, as the history sees it: no members, so only the editor reads it.</summary>
    public interface ISnapshot
    {
    }

    /// <summary>The text and the cursor. The class is private, so only the editor can read it.</summary>
    private sealed class Snapshot : ISnapshot
    {
        public string Text { get; }
        public int Cursor { get; }

        public Snapshot(string text, int cursor)
        {
            Text = text;
            Cursor = cursor;
        }
    }

    private string text = "";
    private int cursor;

    public void Type(string words)
    {
        text = text.Substring(0, cursor) + words + text.Substring(cursor);
        cursor += words.Length;
    }

    public void MoveCursor(int position)
    {
        cursor = Math.Max(0, Math.Min(position, text.Length));
    }

    public ISnapshot Save()
    {
        return new Snapshot(text, cursor);
    }

    /// <summary>A snapshot of any other class throws <see cref="InvalidCastException"/>.</summary>
    public void Restore(ISnapshot snapshot)
    {
        Snapshot saved = (Snapshot)snapshot;
        text = saved.Text;
        cursor = saved.Cursor;
    }

    public override string ToString()
    {
        return text.Substring(0, cursor) + "|" + text.Substring(cursor);
    }
}
