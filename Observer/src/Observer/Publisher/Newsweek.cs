namespace dev.kaldiroglu.Observer.Publisher;

public class Newsweek : AbstractPublication
{
    public Newsweek(string name) : base(name)
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
}
