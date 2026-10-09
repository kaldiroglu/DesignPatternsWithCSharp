namespace dev.kaldiroglu.Memento.Pattern1;

/// <summary>
/// The <b>Caretaker</b>: on its own thread, it keeps the mementos and decides when to save and
/// when to undo. It never reads the state inside a memento for itself; it only prints it.
/// </summary>
/// <remarks>
/// In the Java, <c>Caretaker</c> extends <c>Thread</c>. In C# <c>Thread</c> is <c>sealed</c>,
/// so the caretaker holds a thread instead. <see cref="Start"/> starts it, and the thread
/// calls <see cref="Run"/>.
/// </remarks>
public class Caretaker
{
    private readonly Originator originator;
    private readonly Stack<Memento> history = new();
    private readonly Thread thread;

    public Caretaker(Originator originator)
    {
        this.originator = originator;
        thread = new Thread(Run);
    }

    /// <summary>Starts the caretaker's thread, which calls <see cref="Run"/>.</summary>
    public void Start()
    {
        thread.Start();
    }

    /// <summary>Waits until the caretaker's thread ends.</summary>
    public void Join()
    {
        thread.Join();
    }

    public void Save()
    {
        Memento memento = originator.CreateMemento();
        history.Push(memento);
        Console.WriteLine("Caretaker: Saving state: " + memento.State);
    }

    /// <summary>Goes back to the state saved before the last one.</summary>
    public void Undo()
    {
        if (history.Count < 2)
        {
            Console.Error.WriteLine("Caretaker: Nothing to undo.");
            return;
        }
        history.Pop();
        Memento previous = history.Peek();
        originator.Restore(previous);
        Console.Error.WriteLine("Caretaker: Undoing to: " + previous.State);
    }

    public void Run()
    {
        for (int i = 0; i < 11; i++)
        {
            if (i != 0 && i % 5 == 0)
                Undo();
            else
                Save();
            try
            {
                Thread.Sleep(2000);
            }
            catch (ThreadInterruptedException e)
            {
                Console.Error.WriteLine(e);
            }
        }
    }
}
