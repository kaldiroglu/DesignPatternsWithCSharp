namespace dev.kaldiroglu.Memento.Tests.Pattern1;

using dev.kaldiroglu.Memento.Pattern1;
using Xunit;
using static dev.kaldiroglu.Memento.Tests.Printed;

/// <summary>The caretaker keeps the mementos; each undo goes back exactly one save.</summary>
public class CaretakerTest
{
    /// <summary>Main undoes to the last save, then to the one before it.</summary>
    [Fact]
    public void MainUndoesOneSaveAtATime()
    {
        Assert.Equal(new[]
        {
            "New state: state-1",
            "Caretaker: Saving state: state-1",
            "New state: state-2",
            "New state: state-3",
            "Caretaker: Saving state: state-3",
            "New state: state-4",
            "Caretaker: Undoing to: state-3",
            "Caretaker: Undoing to: state-1",
            "Caretaker: Nothing to undo.",
            "Final: Originator [state=state-1]"
        }, By(Main.Run));
    }

    /// <summary>One undo after one save gives back the saved state, not an older one.</summary>
    [Fact]
    public void UndoRestoresTheLastSave()
    {
        var originator = new Originator("a");
        var caretaker = new Caretaker(originator);
        By(() =>
        {
            originator.SetState("b");
            caretaker.Save();
            originator.SetState("c");
            caretaker.Undo();
        });
        Assert.Equal("Originator [state=b]", originator.ToString());
    }

    /// <summary>The example runs on one thread.</summary>
    [Fact]
    public void NoThreads()
    {
        foreach (var file in new[] { "Caretaker.cs", "Main.cs", "Originator.cs" })
        {
            string code = CodeOf("Pattern1/" + file);
            Assert.DoesNotContain("Thread", code);
            Assert.DoesNotContain("lock (", code);
        }
    }
}
