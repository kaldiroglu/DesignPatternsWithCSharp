namespace dev.kaldiroglu.State.Gof.Problem;

/// <summary>
/// Before the pattern: a TCP connection that switches on its own state in every operation.
/// <para>
/// GoF's motivation (p. 305): a connection can be established, listening or closed, and it
/// answers each request differently in each state. Here every operation is a switch over the
/// three states, so the rules of one state are spread over five methods.
/// </para>
/// </summary>
public sealed class TCPConnection
{
    /// <summary>
    /// The three states. Java calls this nested enum <c>State</c>; in C# a class cannot have
    /// a nested type and a property with the same name, and the property <see cref="State"/>
    /// is the one callers use.
    /// </summary>
    private enum ConnectionState { CLOSED, LISTEN, ESTABLISHED }

    private ConnectionState state = ConnectionState.CLOSED;
    private readonly List<string> log = [];

    public void ActiveOpen()
    {
        switch (state)
        {
            case ConnectionState.CLOSED:
                log.Add("send SYN");
                state = ConnectionState.ESTABLISHED;
                break;
            case ConnectionState.LISTEN:
            case ConnectionState.ESTABLISHED:
                log.Add("ignored: activeOpen");
                break;
        }
    }

    public void PassiveOpen()
    {
        switch (state)
        {
            case ConnectionState.CLOSED:
                state = ConnectionState.LISTEN;
                break;
            case ConnectionState.LISTEN:
            case ConnectionState.ESTABLISHED:
                log.Add("ignored: passiveOpen");
                break;
        }
    }

    public void Send(string data)
    {
        switch (state)
        {
            case ConnectionState.LISTEN:
                log.Add("send SYN, SYN-ACK");
                state = ConnectionState.ESTABLISHED;
                break;
            case ConnectionState.ESTABLISHED:
                log.Add("sent: " + data);
                break;
            case ConnectionState.CLOSED:
                log.Add("ignored: send");
                break;
        }
    }

    public void Acknowledge()
    {
        switch (state)
        {
            case ConnectionState.ESTABLISHED:
                log.Add("ACK");
                break;
            case ConnectionState.CLOSED:
            case ConnectionState.LISTEN:
                log.Add("ignored: acknowledge");
                break;
        }
    }

    public void Close()
    {
        switch (state)
        {
            case ConnectionState.ESTABLISHED:
                log.Add("send FIN");
                state = ConnectionState.LISTEN;
                break;
            case ConnectionState.CLOSED:
            case ConnectionState.LISTEN:
                log.Add("ignored: close");
                break;
        }
    }

    public string State => state.ToString();

    public IReadOnlyList<string> Log => log.ToList().AsReadOnly();
}
