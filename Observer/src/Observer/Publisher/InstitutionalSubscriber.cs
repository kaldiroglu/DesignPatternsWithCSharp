namespace dev.kaldiroglu.Observer.Publisher;

public class InstitutionalSubscriber : AbstractSubscriber
{
    public InstitutionalSubscriber(string name) : base(name)
    {
    }

    public override void Receive(IPublication publication)
    {
        PutOnShelf(publication);
    }

    public void PutOnShelf(IPublication publication)
    {
        Console.WriteLine(publication.Issue + " is on the shelf of " + name);
    }
}
