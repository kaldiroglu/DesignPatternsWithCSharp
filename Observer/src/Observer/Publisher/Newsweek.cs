namespace dev.kaldiroglu.Observer.Publisher;

public class Newsweek : AbstractPublication
{
    public Newsweek(string name) : base(name)
    {
    }

    public override void Publish(string date)
    {
        // NOTE: the date is added to the name on every call, so the second issue is named
        // "Newsweek - <date 1> - <date 2>". The Java has the same behavior; it is kept so the
        // output matches.
        name = name + " - " + date;
        using var iterator = subscribers.GetEnumerator();
        while (iterator.MoveNext())
        {
            ISubscriber subscriber = iterator.Current;
            subscriber.Receive(this);
        }
    }
}
