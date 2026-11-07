namespace dev.kaldiroglu.Memento.Pattern2;

/// <summary>
/// The <b>Caretaker</b>: it keeps the mementos and decides when to save and when to undo. It
/// cannot read the state inside a memento, so it prints the originator instead.
/// </summary>
public class Caretaker
{
    private readonly Originator originator;
    private readonly Stack<Originator.IMemento> history = new();

    public Caretaker(Originator originator)
    {
        this.originator = originator;
    }

    public void Save()
    {
        history.Push(originator.CreateMemento());
        Console.WriteLine("Caretaker: Saved memento " + history.Count + ": " + originator);
    }

    /// <summary>Goes back to the last saved state, and forgets that save.</summary>
    public void Undo()
    {
        if (history.Count == 0)
        {
            Console.WriteLine("Caretaker: Nothing to undo.");
            return;
        }
        originator.Restore(history.Pop());
        Console.WriteLine("Caretaker: Undid to: " + originator);
    }
}
