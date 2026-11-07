namespace dev.kaldiroglu.Memento.Pattern1;

/// <summary>
/// The <b>Caretaker</b>: it keeps the mementos and decides when to save and when to undo. It
/// reads the state inside a memento only to print it, which this namespace allows because the
/// memento's <see cref="Memento.State"/> is public.
/// </summary>
public class Caretaker
{
    private readonly Originator originator;
    private readonly Stack<Memento> history = new();

    public Caretaker(Originator originator)
    {
        this.originator = originator;
    }

    public void Save()
    {
        Memento memento = originator.CreateMemento();
        history.Push(memento);
        Console.WriteLine("Caretaker: Saving state: " + memento.State);
    }

    /// <summary>Goes back to the last saved state, and forgets that save.</summary>
    public void Undo()
    {
        if (history.Count == 0)
        {
            Console.WriteLine("Caretaker: Nothing to undo.");
            return;
        }
        Memento last = history.Pop();
        originator.Restore(last);
        Console.WriteLine("Caretaker: Undoing to: " + last.State);
    }
}
