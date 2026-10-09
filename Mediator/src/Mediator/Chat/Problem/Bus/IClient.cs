namespace dev.kaldiroglu.Mediator.Chat.Problem.Bus;

/// <summary>Anything that subscribes to the bus.</summary>
public interface IClient
{
    string Name { get; }

    void OnMessage(Message message);
}
