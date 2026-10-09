namespace dev.kaldiroglu.Memento.Hw.Editor;

/// <summary>
/// The <b>Caretaker</b>: undo and redo with two stacks of mementos.
/// <para>
/// Before every change, the current snapshot goes on the undo stack. Undo moves the current
/// snapshot to the redo stack and restores the last one; redo does the opposite. A new change
/// clears the redo stack.
/// </para>
/// </summary>
public sealed class History
{
    private readonly TextEditor editor;
    private readonly Stack<TextEditor.ISnapshot> undo = new Stack<TextEditor.ISnapshot>();
    private readonly Stack<TextEditor.ISnapshot> redo = new Stack<TextEditor.ISnapshot>();

    public History(TextEditor editor)
    {
        this.editor = editor;
    }

    public void Type(string words)
    {
        undo.Push(editor.Save());
        redo.Clear();
        editor.Type(words);
    }

    public void Undo()
    {
        if (undo.Count > 0)
        {
            redo.Push(editor.Save());
            editor.Restore(undo.Pop());
        }
    }

    public void Redo()
    {
        if (redo.Count > 0)
        {
            undo.Push(editor.Save());
            editor.Restore(redo.Pop());
        }
    }
}
