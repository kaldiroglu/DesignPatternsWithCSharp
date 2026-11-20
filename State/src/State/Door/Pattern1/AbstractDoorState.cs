namespace dev.kaldiroglu.State.Door.Pattern1;

/// <summary>
/// What both states share: the door they change, and the two states they can move it to.
/// <see cref="Wire"/> is internal, so only <see cref="Door"/> connects them.
/// </summary>
public abstract class AbstractDoorState : IDoorState
{
    // Set by Door through Wire, before any request arrives.
    protected Door door = null!;
    protected IDoorState openState = null!;
    protected IDoorState closedState = null!;

    internal void Wire(Door door, IDoorState openState, IDoorState closedState)
    {
        this.door = door;
        this.openState = openState;
        this.closedState = closedState;
    }

    public abstract void Open();

    public abstract void Close();

    public abstract bool IsOpen { get; }
}
