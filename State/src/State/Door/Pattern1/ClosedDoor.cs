namespace dev.kaldiroglu.State.Door.Pattern1;

/// <summary>The closed state. It knows the open state, and moves the door there itself.</summary>
public class ClosedDoor : AbstractDoorState
{
    public override void Open()
    {
        door.ChangeState(openState);
    }

    public override void Close()
    {
        Console.WriteLine("Door is already closed.");
    }

    public override bool IsOpen => false;
}
