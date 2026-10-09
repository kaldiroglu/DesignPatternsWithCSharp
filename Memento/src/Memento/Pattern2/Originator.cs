namespace dev.kaldiroglu.Memento.Pattern2;

/// <summary>
/// The <b>Originator</b>: holds one state. Its memento is a nested class, so the memento can
/// read the originator's private state.
/// </summary>
public class Originator
{
    private readonly object sync = new object();
    private volatile string state;
    private readonly Memento memento = new Memento();

    public Originator(string state)
    {
        this.state = state;
        memento.SetOriginator(this);
    }

    /// <summary>Sets the state and prints it. It stays a method because it does more than set a value.</summary>
    public void SetState(string state)
    {
        lock (sync)
        {
            Console.WriteLine("\nNew state: " + state);
            this.state = state;
        }
    }

    /// <summary>
    /// Stays a method: a property named <c>Memento</c> would clash with the nested class of the
    /// same name. It is <c>internal</c> because the nested class is <c>internal</c>, as the
    /// Java class is package-private.
    /// </summary>
    internal Memento GetMemento()
    {
        return memento;
    }

    public override string ToString()
    {
        return "Originator [state=" + state + "]";
    }

    /// <summary>
    /// The <b>Memento</b>, nested in the originator. In C#, as in Java, a nested class can read
    /// the private members of the class that encloses it, so it reads <c>state</c> directly.
    /// </summary>
    /// <remarks>
    /// The Java class is an inner class, which has a hidden reference to the originator that
    /// created it. A C# nested class has no such reference. This memento does not use it
    /// anyway: it gets its originator through <see cref="SetOriginator"/>, as the Java does.
    /// </remarks>
    internal class Memento
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
                string state = originator!.state;
                Console.WriteLine("Memento: Saving state: " + state);
                states.Add(state);
                position++;
            }
        }

        public void Undo()
        {
            lock (sync)
            {
                // NOTE: undo goes back two saves and never decreases position, so a second
                // undo goes to the same state as the first. After a single save the index is
                // -1 and this throws ArgumentOutOfRangeException. The Java has the same
                // behavior (there it throws IndexOutOfBoundsException).
                int currentPosition = position;
                currentPosition -= 2;
                string previousState = states[currentPosition];
                originator!.SetState(previousState);
                Console.Error.WriteLine("Memento: Undoing to: " + previousState);
            }
        }
    }
}
