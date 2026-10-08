namespace dev.kaldiroglu.State.Gof.Solution;

/// <summary>
/// The <b>Context</b>: GoF's <c>TCPConnection</c>.
/// <para>
/// It holds one <see cref="TCPState"/> and forwards every request to it, passing itself so
/// that the state can change it. <see cref="ChangeState"/> is <c>internal</c>: only the states
/// should call it. In Java it is package-private; in C++ GoF make <c>TCPState</c> a friend of
/// the connection for the same reason. C#'s <c>internal</c> opens it to the whole assembly,
/// which is the closest C# has.
/// </para>
/// </summary>
public sealed class TCPConnection
{
    private TCPState state = TCPClosed.Instance;
    private readonly List<string> log = [];

    public void ActiveOpen()
    {
        state.ActiveOpen(this);
    }

    public void PassiveOpen()
    {
        state.PassiveOpen(this);
    }

    public void Send(string data)
    {
        state.Send(this, data);
    }

    public void Acknowledge()
    {
        state.Acknowledge(this);
    }

    public void Close()
    {
        state.Close(this);
    }

    public string State => state.Name;

    public IReadOnlyList<string> Log => log.ToList().AsReadOnly();

    internal void ChangeState(TCPState next)
    {
        state = next;
    }

    internal void Record(string line)
    {
        log.Add(line);
    }
}
