namespace dev.kaldiroglu.State.Door.Pattern1;

/// <summary>A <b>ConcreteState</b>: the door is open. Closing it moves the door to the closed state.</summary>
public class OpenDoor() : AbstractDoorState(true)
{
    public override void Open()
    {
        Console.WriteLine("Door is already open!");
    }

    public override void Close()
    {
        door.ChangeState(closedState);
    }
}
