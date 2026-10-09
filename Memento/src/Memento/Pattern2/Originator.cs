namespace dev.kaldiroglu.Memento.Pattern2;

/// <summary>
/// The <b>Originator</b>, with its memento nested inside it.
/// <para>
/// The Java memento is a nested class with a private field: the originator can read it and
/// nobody else can. A C# outer class cannot read a nested class's private members, so here
/// the caretaker holds an empty public interface, <see cref="IMemento"/>, and the state is in a
/// private nested class that only the originator can name. The originator therefore needs no
/// public state getter.
/// </para>
/// </summary>
public class Originator
{
    /// <summary>What the caretaker holds: nothing it can read.</summary>
    public interface IMemento
    {
    }

    private sealed class Memento : IMemento
    {
        public Memento(string state)
        {
            State = state;
        }

        public string State { get; }
    }

    private readonly object sync = new object();
    private volatile string state;

    public Originator(string state)
    {
        this.state = state;
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

    public IMemento CreateMemento()
    {
        lock (sync)
        {
            return new Memento(state);
        }
    }

    /// <summary>Takes back a memento this class created. Any other IMemento throws InvalidCastException.</summary>
    public void Restore(IMemento memento)
    {
        SetState(((Memento)memento).State);
    }

    public override string ToString()
    {
        return "Originator [state=" + state + "]";
    }
}
