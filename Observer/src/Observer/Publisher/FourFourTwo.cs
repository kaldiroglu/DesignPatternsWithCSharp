namespace dev.kaldiroglu.Observer.Publisher;

public class FourFourTwo : AbstractPublication
{
    public FourFourTwo(string name) : base(name)
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
