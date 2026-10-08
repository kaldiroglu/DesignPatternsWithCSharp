namespace dev.kaldiroglu.State.Door.Pattern2;

/// <summary>What both door states share: whether they are open, and the manager they ask.</summary>
public abstract class AbstractDoor(bool open, DoorStateManager dsm) : IDoorState
{
    protected bool open = open;
    protected DoorStateManager dsm = dsm;

    public abstract void Open();

    public abstract void Close();

    public bool IsOpen => open;
}
