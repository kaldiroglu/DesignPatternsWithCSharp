namespace dev.kaldiroglu.Mediator.Hw.BankQueue;

/// <summary>A <b>Colleague</b>: a teller. It knows the queue manager, not the customers.</summary>
public sealed class Teller
{
    private readonly QueueManager manager;

    public Teller(string name, QueueManager manager)
    {
        Name = name;
        this.manager = manager;
    }

    public void Free()
    {
        manager.TellerFree(this);
    }

    public string Name { get; }
}
