namespace dev.kaldiroglu.State.Pattern;

/// <summary>The <b>State</b> of the outline: the same three requests as the connection.</summary>
public interface ITCPState
{
    void Open();

    void Close();

    void Acknowledge();
}
