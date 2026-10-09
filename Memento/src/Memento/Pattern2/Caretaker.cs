namespace dev.kaldiroglu.Memento.Pattern2;

/// <summary>
/// The <b>Caretaker</b>: on its own thread, it keeps the mementos and decides when to save and
/// when to undo. It cannot read the state inside a memento, so it reports only how many it
/// keeps.
/// </summary>
/// <remarks>
/// In the Java, <c>Caretaker</c> extends <c>Thread</c>. In C# <c>Thread</c> is <c>sealed</c>,
/// so the caretaker holds a thread instead.
/// </remarks>
public class Caretaker
{
    private readonly Originator originator;
    private readonly Stack<Originator.IMemento> history = new();
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
        history.Push(originator.CreateMemento());
        Console.WriteLine("Caretaker: Saved memento " + history.Count.ToString(System.Globalization.CultureInfo.InvariantCulture) + ": " + originator);
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
        originator.Restore(history.Peek());
        Console.Error.WriteLine("Caretaker: Undid to memento " + history.Count.ToString(System.Globalization.CultureInfo.InvariantCulture) + ": " + originator);
    }

    public void Run()
    {
        for (int i = 0; i < 10; i++)
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
