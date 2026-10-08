namespace dev.kaldiroglu.State.Door.Pattern1;

/// <summary>A <b>ConcreteState</b>: the door is closed. Opening it moves the door to the open state.</summary>
public class ClosedDoor() : AbstractDoorState(false)
{
    public override void Open()
    {
        door.ChangeState(openState);
    }

    public override void Close()
    {
        Console.WriteLine("Door is already closed!");
    }
}
