namespace dev.kaldiroglu.State.Door.Pattern2;

/// <summary>A <b>ConcreteState</b>: the door is open. To close it, it asks the manager.</summary>
public class OpenDoor(DoorStateManager dsm) : AbstractDoor(true, dsm)
{
    public override void Open()
    {
        Console.WriteLine("Door is already open.");
    }

    public override void Close()
    {
        dsm.CloseDoor();
    }
}
