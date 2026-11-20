namespace dev.kaldiroglu.State.Door.Pattern2;

/// <summary>The <b>State</b>: it does the work of a request. It does not choose the next state.</summary>
public interface IDoorState
{
    void Open();

    void Close();

    bool IsOpen { get; }
}
