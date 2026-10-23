namespace dev.kaldiroglu.Observer.Publisher;

public class FourFourTwo : AbstractPublication
{
    /// <summary>
    /// <c>protected internal</c>, because <see cref="Publisher"/> creates it from the same
    /// assembly.
    /// </summary>
    protected internal FourFourTwo(string name) : base(name)
    {
    }

    public override void Publish(string date)
    {
        issueDate = date;
        using var iterator = subscribers.GetEnumerator();
        while (iterator.MoveNext())
        {
            ISubscriber subscriber = iterator.Current;
            subscriber.Receive(this);
        }
    }

    public override void ListSubscribers()
    {
        using var iterator = subscribers.GetEnumerator();
        while (iterator.MoveNext())
            Console.WriteLine(iterator.Current);
    }
}
