namespace dev.kaldiroglu.Mediator.Hw.BankQueue;

/// <summary>A <b>Colleague</b>: a customer. It knows the queue manager, not the tellers.</summary>
public sealed class Customer
{
    private readonly QueueManager manager;

    public Customer(string name, QueueManager manager)
    {
        Name = name;
        this.manager = manager;
    }

    public void Arrive()
    {
        manager.Arrive(this);
    }

    public string Name { get; }

    /// <summary>The number the customer took. Only the queue manager sets it.</summary>
    public int Number { get; internal set; }
}
