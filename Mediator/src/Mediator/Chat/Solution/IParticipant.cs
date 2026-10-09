namespace dev.kaldiroglu.Mediator.Chat.Solution;

/// <summary>
/// The <b>Colleague</b>: someone in the chat. It knows the room, never another participant.
/// The room calls <see cref="Receive"/>; nothing else does.
/// </summary>
public interface IParticipant
{
    string Name { get; }

    void Receive(string from, string text);
}
