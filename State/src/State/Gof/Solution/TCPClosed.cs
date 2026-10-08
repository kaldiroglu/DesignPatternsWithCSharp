namespace dev.kaldiroglu.State.Gof.Solution;

/// <summary>
/// A <b>ConcreteState</b>. It has no fields, so one object serves every connection: GoF make
/// each state a Singleton. That is GoF implementation issue 2 (creating and destroying State
/// objects): create a state once and share it, when it holds nothing of its own.
/// </summary>
public sealed class TCPClosed : TCPState
{
    public static readonly TCPClosed Instance = new();

    private TCPClosed()
    {
    }

    public override string Name => "CLOSED";

    public override void ActiveOpen(TCPConnection connection)
    {
        connection.Record("send SYN");
        ChangeState(connection, TCPEstablished.Instance);
    }

    public override void PassiveOpen(TCPConnection connection)
    {
        ChangeState(connection, TCPListen.Instance);
    }
}
