namespace dev.kaldiroglu.State.Gof.Solution;

/// <summary>A <b>ConcreteState</b>: waiting for the other side. Sending opens the connection.</summary>
public sealed class TCPListen : TCPState
{
    public static readonly TCPListen Instance = new();

    private TCPListen()
    {
    }

    public override string Name => "LISTEN";

    public override void Send(TCPConnection connection, string data)
    {
        connection.Record("send SYN, SYN-ACK");
        ChangeState(connection, TCPEstablished.Instance);
    }
}
