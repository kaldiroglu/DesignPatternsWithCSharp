namespace dev.kaldiroglu.State.Door.Pattern1;

/// <summary>The <b>State</b>: only the requests a door takes. The wiring is not part of it.</summary>
public interface IDoorState
{
    void Open();

    void Close();

    bool IsOpen { get; }
}
