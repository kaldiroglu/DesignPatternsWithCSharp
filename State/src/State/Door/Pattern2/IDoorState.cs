namespace dev.kaldiroglu.State.Door.Pattern2;

/// <summary>The <b>State</b>. In this version a state knows only the manager, not the other state.</summary>
public interface IDoorState
{
    void Open();

    void Close();

    bool IsOpen { get; }
}
