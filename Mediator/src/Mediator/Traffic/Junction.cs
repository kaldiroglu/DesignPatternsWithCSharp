namespace dev.kaldiroglu.Mediator.Traffic;

/// <summary>The shared resource: a junction that one car at a time may pass.</summary>
public class Junction
{
    // NOTE: busy is read and written by several threads, and it is not volatile, so a thread
    // may not see another thread's change at once. The Java field is not volatile either, and
    // has the same behavior.
    private bool busy;

    public Junction(string name)
    {
        Name = name;
        busy = false;
        Console.WriteLine("Junction " + name + " created.");
    }

    public bool IsBusy()
    {
        return busy;
    }

    public void SetBusy(bool busy)
    {
        this.busy = busy;
    }

    public string Name { get; }
}
