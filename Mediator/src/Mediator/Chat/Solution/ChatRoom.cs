namespace dev.kaldiroglu.Mediator.Chat.Solution;

/// <summary>
/// The <b>Mediator</b>: the only way one participant reaches another.
/// <para>
/// Every rule about who receives what is here, in one class: a message to everyone goes to
/// all but the sender, a private message goes to its receiver only, and nobody receives
/// from someone they blocked. A participant written later — a guest client — gets only what
/// the room sends it, so it cannot show what was not meant for it.
/// </para>
/// </summary>
public sealed class ChatRoom
{
    // OrderedDictionary keeps the order in which participants joined, as Java's LinkedHashMap
    // does. A plain Dictionary does not promise any order.
    private readonly OrderedDictionary<string, IParticipant> participants = new();
    private readonly Dictionary<string, HashSet<string>> blocks = new();
    private readonly List<string> deliveries = [];

    public void Join(IParticipant participant)
    {
        participants[participant.Name] = participant;
    }

    public void Block(string blocker, string blocked)
    {
        if (!blocks.TryGetValue(blocker, out HashSet<string>? names))
        {
            names = [];
            blocks[blocker] = names;
        }
        names.Add(blocked);
    }

    public void Say(string from, string text)
    {
        foreach (IParticipant p in participants.Values)
        {
            if (p.Name != from && !HasBlocked(p.Name, from))
            {
                Deliver(p, from, text);
            }
        }
    }

    public void Whisper(string from, string to, string text)
    {
        if (participants.TryGetValue(to, out IParticipant? p) && !HasBlocked(to, from))
        {
            Deliver(p, from + " (private)", text);
        }
    }

    private bool HasBlocked(string blocker, string sender)
    {
        return blocks.TryGetValue(blocker, out HashSet<string>? names) && names.Contains(sender);
    }

    private void Deliver(IParticipant p, string from, string text)
    {
        deliveries.Add(p.Name);
        p.Receive(from, text);
    }

    /// <summary>The participants each message was delivered to, in order.</summary>
    public IReadOnlyList<string> Deliveries => deliveries.ToList();
}
