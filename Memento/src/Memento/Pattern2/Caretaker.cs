namespace dev.kaldiroglu.Memento.Pattern2;

/// <summary>
/// The <b>Caretaker</b>: on its own thread, it saves every two seconds and undoes every fifth
/// time.
/// </summary>
/// <remarks>
/// In the Java, <c>Caretaker</c> extends <c>Thread</c>. In C# <c>Thread</c> is <c>sealed</c>,
/// so the caretaker holds a thread instead. <see cref="Start"/> starts it, and the thread
/// calls <see cref="Run"/>.
/// </remarks>
public class Caretaker
{
    private readonly Originator.Memento memento;
    private readonly Thread thread;

    internal Caretaker(Originator.Memento memento)
    {
        this.memento = memento;
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

    public void Run()
    {
        for (int i = 0; i < 10; i++)
        {
            if (i != 0 && i % 5 == 0)
                memento.Undo();
            else
                memento.Save();
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
