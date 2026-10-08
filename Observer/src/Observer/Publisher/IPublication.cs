namespace dev.kaldiroglu.Observer.Publisher;

/// <summary>The Subject: a publication that keeps its subscribers and sends them each new issue.</summary>
public interface IPublication
{
    string Name { get; }

    void AddSubscriber(ISubscriber subscriber);

    void RemoveSubscriber(ISubscriber subscriber);

    void Publish(string date);

    void ListSubscribers();
}
