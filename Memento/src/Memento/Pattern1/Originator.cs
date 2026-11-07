namespace dev.kaldiroglu.Memento.Pattern1;

/// <summary>The <b>Originator</b>: it creates a memento of its state, and restores itself from one.</summary>
public class Originator
{
    private string state;

    public Originator(string state)
    {
        this.state = state;
    }

    public string State => state;

    /// <summary>Sets the state and prints it. It stays a method because it does more than set a value.</summary>
    public void SetState(string state)
    {
        Console.WriteLine("New state: " + state);
        this.state = state;
    }

    public Memento CreateMemento()
    {
        return new Memento(state);
    }

    public void Restore(Memento memento)
    {
        state = memento.State;
    }

    public override string ToString()
    {
        return "Originator [state=" + state + "]";
    }
}
