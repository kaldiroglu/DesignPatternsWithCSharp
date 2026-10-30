namespace dev.kaldiroglu.Observer.Publisher;

public abstract class AbstractPublication : IPublication
{
    protected string name;
    protected string? issueDate;
    protected List<ISubscriber> subscribers;

    protected AbstractPublication(string name)
    {
        this.name = name;
        subscribers = [];
    }

    public string Name => name;

    public string Issue => name + " - " + issueDate;

    public void AddSubscriber(ISubscriber subscriber)
    {
        subscribers.Add(subscriber);
    }

    public void RemoveSubscriber(ISubscriber subscriber)
    {
        subscribers.Remove(subscriber);
    }

    /// <summary>
    /// Left to the subclasses. A C# abstract class must declare every interface member, so
    /// it is declared abstract here.
    /// </summary>
    public abstract void Publish(string date);

    public void ListSubscribers()
    {
        foreach (ISubscriber subscriber in subscribers)
        {
            Console.WriteLine(subscriber.Name);
        }
    }
}
