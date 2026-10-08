namespace dev.kaldiroglu.State.Door.Pattern2;

/// <summary>A <b>ConcreteState</b>: the door is closed. To open it, it asks the manager.</summary>
public class ClosedDoor(DoorStateManager dsm) : AbstractDoor(false, dsm)
{
    public override void Open()
    {
        dsm.OpenDoor();
    }

    public override void Close()
    {
        Console.WriteLine("Door is already closed.");
    }
}
