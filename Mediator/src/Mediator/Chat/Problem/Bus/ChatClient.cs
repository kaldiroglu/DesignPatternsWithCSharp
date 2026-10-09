namespace dev.kaldiroglu.Mediator.Chat.Problem.Bus;

/// <summary>The team's own client. It shows a private message only if it is addressed to it.</summary>
public sealed class ChatClient : IClient
{
    private readonly List<string> shown = [];

    public ChatClient(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public void OnMessage(Message message)
    {
        if (message.IsPrivate && message.To != Name)
        {
            return;                       // not for me
        }
        shown.Add(message.From + (message.IsPrivate ? " (private)" : "") + ": " + message.Text);
    }

    /// <summary>A copy of what the client shows, as Java's <c>List.copyOf</c> gives.</summary>
    public IReadOnlyList<string> Shown => shown.ToList();
}
