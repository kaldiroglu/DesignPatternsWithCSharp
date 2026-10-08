namespace dev.kaldiroglu.State.Pattern;

/// <summary>An earlier outline of GoF's TCP example: the connection's three requests. See <c>Gof</c> for the full version.</summary>
public interface ITCPConnection
{
    void Open();

    void Close();

    void Acknowledge();
}
