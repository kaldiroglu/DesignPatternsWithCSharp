namespace dev.kaldiroglu.State.Gof.Solution;

/// <summary>
/// The <b>State</b>: GoF's <c>TCPState</c>.
/// <para>
/// It is an abstract class, not an interface, because it gives every operation a default:
/// ignore the request. Each concrete state overrides only the requests it answers. That is
/// how GoF write it.
/// </para>
/// </summary>
public abstract class TCPState
{
    public abstract string Name { get; }

    public virtual void ActiveOpen(TCPConnection connection)
    {
        connection.Record("ignored: activeOpen");
    }

    public virtual void PassiveOpen(TCPConnection connection)
    {
        connection.Record("ignored: passiveOpen");
    }

    public virtual void Send(TCPConnection connection, string data)
    {
        connection.Record("ignored: send");
    }

    public virtual void Acknowledge(TCPConnection connection)
    {
        connection.Record("ignored: acknowledge");
    }

    public virtual void Close(TCPConnection connection)
    {
        connection.Record("ignored: close");
    }

    /// <summary>GoF's protected <c>ChangeState</c>: a state tells the connection which state comes next.</summary>
    protected void ChangeState(TCPConnection connection, TCPState next)
    {
        connection.ChangeState(next);
    }
}
