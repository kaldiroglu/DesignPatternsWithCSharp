namespace dev.kaldiroglu.Mediator.Chat.Problem.Bus;

/// <summary>
/// A client for guests, written later by another team. It shows what the bus gives it.
/// <para>
/// It does not check <c>To</c>, so it shows private messages meant for someone else.
/// Nothing fails: the bus delivered the message, and the client displayed it.
/// </para>
/// </summary>
public sealed class GuestClient : IClient
{
    private readonly List<string> shown = [];

    public GuestClient(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public void OnMessage(Message message)
    {
        shown.Add(message.From + ": " + message.Text);
    }

    /// <summary>A copy of what the client shows, as Java's <c>List.copyOf</c> gives.</summary>
    public IReadOnlyList<string> Shown => shown.ToList();
}
