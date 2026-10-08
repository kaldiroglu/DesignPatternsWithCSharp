namespace dev.kaldiroglu.Observer.Publisher;

public class IndividualSubscriber : AbstractSubscriber
{
    public IndividualSubscriber(string name) : base(name)
    {
    }

    public override void Receive(IPublication publication)
    {
        Read(publication);
    }

    public void Read(IPublication publication)
    {
        Console.WriteLine(name + " is reading " + publication.Name);
    }
}
