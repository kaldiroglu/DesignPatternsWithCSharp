namespace dev.kaldiroglu.State.Door.Pattern1;

/// <summary>
/// What both door states share: the door they change, whether they are open, and the other
/// state to move to.
/// </summary>
public abstract class AbstractDoorState(bool open) : IDoorState
{
    // Set by Door right after it creates the states, before any request arrives.
    protected Door door = null!;
    protected bool open = open;

    // Each state is given only the other one; the field for its own state stays empty.
    protected IDoorState openState = null!;
    protected IDoorState closedState = null!;

    public abstract void Open();

    public abstract void Close();

    public void SetClosedState(IDoorState closedState)
    {
        this.closedState = closedState;
    }

    public void SetOpenState(IDoorState openState)
    {
        this.openState = openState;
    }

    public bool IsOpen => open;

    public void SetDoor(Door door)
    {
        this.door = door;
    }
}
