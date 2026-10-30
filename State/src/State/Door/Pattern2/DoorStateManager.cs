namespace dev.kaldiroglu.State.Door.Pattern2;

/// <summary>
/// The transitions in one central place. Only the manager knows all the states; a state asks
/// the manager for the next state, and does not name it.
/// </summary>
public class DoorStateManager
{
    private readonly Door door;

    private readonly IDoorState openState;
    private readonly IDoorState closedState;

    public DoorStateManager(Door door)
    {
        this.door = door;
        openState = new OpenDoor(this);
        closedState = new ClosedDoor(this);
        door.ChangeState(closedState);
    }

    /// <summary>The one place that says which state follows which.</summary>
    public void Next(IDoorState current)
    {
        door.ChangeState(current == openState ? closedState : openState);
    }
}
