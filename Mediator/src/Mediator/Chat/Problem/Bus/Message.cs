namespace dev.kaldiroglu.Mediator.Chat.Problem.Bus;

/// <summary>A message on the bus. <c>To</c> is <c>null</c> for a message to everyone.</summary>
public sealed record Message(string From, string? To, string Text)
{
    public bool IsPrivate => To != null;
}
