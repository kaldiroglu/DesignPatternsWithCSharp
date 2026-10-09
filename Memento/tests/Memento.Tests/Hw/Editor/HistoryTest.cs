namespace dev.kaldiroglu.Memento.Tests.Hw.Editor;

using dev.kaldiroglu.Memento.Hw.Editor;
using Xunit;

/// <summary>Homework 1: undo and redo with two stacks of snapshots.</summary>
public class HistoryTest
{
    /// <summary>Undo goes back one step at a time, and redo goes forward again.</summary>
    [Fact]
    public void UndoAndRedo()
    {
        TextEditor editor = new TextEditor();
        History history = new History(editor);
        history.Type("Hello");
        history.Type(" world");
        Assert.Equal("Hello world|", editor.ToString());
        history.Undo();
        Assert.Equal("Hello|", editor.ToString());
        history.Undo();
        Assert.Equal("|", editor.ToString());
        history.Redo();
        Assert.Equal("Hello|", editor.ToString());
    }

    /// <summary>A new change clears the redo stack.</summary>
    [Fact]
    public void ANewChangeClearsRedo()
    {
        TextEditor editor = new TextEditor();
        History history = new History(editor);
        history.Type("Hello");
        history.Undo();
        history.Type("Hi");
        history.Redo();
        Assert.Equal("Hi|", editor.ToString());
    }

    /// <summary>The snapshot keeps the cursor as well as the text.</summary>
    [Fact]
    public void TheCursorIsSaved()
    {
        TextEditor editor = new TextEditor();
        History history = new History(editor);
        history.Type("Hello");
        editor.MoveCursor(0);
        history.Type(">");
        Assert.Equal(">|Hello", editor.ToString());
        history.Undo();
        Assert.Equal("|Hello", editor.ToString());
    }
}
