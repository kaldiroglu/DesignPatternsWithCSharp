namespace dev.kaldiroglu.Observer.Publisher;

public class Newsweek : AbstractPublication
{
    public Newsweek(string name) : base(name)
    {
    }

    public override void Publish(string date)
    {
        issueDate = date;
        foreach (ISubscriber subscriber in subscribers.ToArray())
        {
            subscriber.Receive(this);
        }
    }
}
