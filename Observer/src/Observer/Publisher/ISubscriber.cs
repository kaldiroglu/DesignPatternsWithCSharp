namespace dev.kaldiroglu.Observer.Publisher;

/// <summary>The Observer: anyone who receives the issues of a publication.</summary>
public interface ISubscriber
{
    string Name { get; }

    void Receive(IPublication publication);
}
