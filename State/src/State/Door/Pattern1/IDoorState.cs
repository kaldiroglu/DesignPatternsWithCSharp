namespace dev.kaldiroglu.State.Door.Pattern1;

/// <summary>
/// The <b>State</b>. Besides the door's two requests, each state is told about the door and
/// about the other state, because in this version the states decide the transitions.
/// </summary>
public interface IDoorState
{
    void Open();

    void Close();

    bool IsOpen { get; }

    void SetDoor(Door door);

    void SetClosedState(IDoorState closedState);

    void SetOpenState(IDoorState openState);
}
