namespace dev.kaldiroglu.Memento.Pattern1;

/// <summary>
/// The <b>Memento</b>: keeps every state the originator had when it was saved, and can undo
/// to an earlier one.
/// </summary>
/// <remarks>
/// The class has the same name as the root namespace <c>dev.kaldiroglu.Memento</c>. Inside
/// this namespace the plain name <c>Memento</c> means this class, because a type in the
/// current namespace is found before an enclosing namespace.
/// </remarks>
public class Memento
{
    private readonly object sync = new object();
    private Originator? originator;
    private readonly List<string> states;
    private int position = 0;

    public Memento()
    {
        states = new List<string>();
    }

    public void SetOriginator(Originator originator)
    {
        this.originator = originator;
    }

    public void Save()
    {
        lock (sync)
        {
            string state = originator!.State;
            Console.WriteLine("Memento: Saving state: " + state);
            states.Add(state);
            position++;
        }
    }

    public void Undo()
    {
        lock (sync)
        {
            // NOTE: undo goes back two saves and never decreases position, so a second undo
            // goes to the same state as the first. After a single save the index is -1 and
            // this throws ArgumentOutOfRangeException. The Java has the same behavior (there
            // it throws IndexOutOfBoundsException).
            int currentPosition = position;
            currentPosition -= 2;
            string previousState = states[currentPosition];
            originator!.SetState(previousState);
            Console.Error.WriteLine("Memento: Undoing to: " + previousState);
        }
    }
}
