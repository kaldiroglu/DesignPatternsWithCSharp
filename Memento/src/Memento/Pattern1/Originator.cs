namespace dev.kaldiroglu.Memento.Pattern1;

/// <summary>The <b>Originator</b>: holds one state, and creates its memento.</summary>
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

    public string State => state;

    /// <summary>Sets the state and prints it. It stays a method because it does more than set a value.</summary>
    public void SetState(string state)
    {
        lock (sync)
        {
            Console.WriteLine("\nNew state: " + state);
            this.state = state;
        }
    }

    public Memento Memento => memento;

    public override string ToString()
    {
        return "Originator [state=" + state + "]";
    }
}
