namespace dev.kaldiroglu.Mediator.Chat.Problem.Bus;

/// <summary>
/// Stage three: a message bus. Clients publish to it and subscribe to it; no client knows
/// another.
/// <para>
/// The bus delivers every message to every subscriber except the sender. Each client then
/// decides what to show. The decision "who may read this" is made by the receivers — so it
/// is only as good as the least careful client.
/// </para>
/// </summary>
public sealed class MessageBus
{
    private readonly List<IClient> subscribers = [];
    private readonly List<string> deliveries = [];

    public void Subscribe(IClient client)
    {
        subscribers.Add(client);
    }

    public void Publish(Message message)
    {
        foreach (IClient client in subscribers.ToList())
        {
            if (client.Name != message.From)
            {
                deliveries.Add(client.Name);
                client.OnMessage(message);
            }
        }
    }

    /// <summary>The clients each message was delivered to, in order.</summary>
    public IReadOnlyList<string> Deliveries => deliveries.ToList();
}
