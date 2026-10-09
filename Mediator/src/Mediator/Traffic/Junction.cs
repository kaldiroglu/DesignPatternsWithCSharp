namespace dev.kaldiroglu.Mediator.Traffic;

/// <summary>The shared resource: a junction that one car at a time may pass.</summary>
public class Junction
{
    // Read and written only inside TrafficPolice's lock.
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
