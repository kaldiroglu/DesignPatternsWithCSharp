namespace dev.kaldiroglu.State.Door.Pattern2;

/// <summary>
/// The transitions in one central place. Only the manager knows all the states; a state asks
/// the manager to open or close the door.
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
    }

    public void ChangeState(IDoorState newState)
    {
        door.ChangeState(newState);
    }

    public void CloseDoor()
    {
        ChangeState(closedState);
    }

    public void OpenDoor()
    {
        ChangeState(openState);
    }
}
