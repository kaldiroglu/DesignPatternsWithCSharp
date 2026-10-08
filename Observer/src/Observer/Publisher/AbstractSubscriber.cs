namespace dev.kaldiroglu.Observer.Publisher;

public abstract class AbstractSubscriber : ISubscriber
{
    protected string name;

    public AbstractSubscriber(string name)
    {
        this.name = name;
    }

    public string Name => name;

    /// <summary>
    /// The Java abstract class leaves <c>receive</c> to its subclasses without naming it. A C#
    /// abstract class must declare every interface member, so it is declared abstract here.
    /// </summary>
    public abstract void Receive(IPublication publication);
}
