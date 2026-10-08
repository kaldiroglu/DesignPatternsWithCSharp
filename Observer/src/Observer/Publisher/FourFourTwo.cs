namespace dev.kaldiroglu.Observer.Publisher;

public class FourFourTwo : AbstractPublication
{
    /// <summary>
    /// <c>protected internal</c>: in Java, <c>protected</c> also opens the constructor to its
    /// package, and <see cref="Publisher"/> uses it from there.
    /// </summary>
    protected internal FourFourTwo(string name) : base(name)
    {
    }

    public override void Publish(string date)
    {
        // NOTE: the date is added to the name on every call, so the second issue is named
        // "FourFourTwo - <date 1> - <date 2>". The Java has the same behavior; it is kept so
        // the output matches.
        name = name + " - " + date;
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
