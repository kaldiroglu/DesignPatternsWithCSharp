namespace dev.kaldiroglu.State.Gof.Solution;

/// <summary>A <b>ConcreteState</b>: the connection is open. It sends data, acknowledges, and closes.</summary>
public sealed class TCPEstablished : TCPState
{
    public static readonly TCPEstablished Instance = new();

    private TCPEstablished()
    {
    }

    public override string Name => "ESTABLISHED";

    public override void Send(TCPConnection connection, string data)
    {
        connection.Record("sent: " + data);
    }

    public override void Acknowledge(TCPConnection connection)
    {
        connection.Record("ACK");
    }

    public override void Close(TCPConnection connection)
    {
        connection.Record("send FIN");
        ChangeState(connection, TCPListen.Instance);
    }
}
