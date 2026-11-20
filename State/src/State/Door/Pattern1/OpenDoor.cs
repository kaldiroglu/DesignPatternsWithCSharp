namespace dev.kaldiroglu.State.Door.Pattern1;

/// <summary>The open state. It knows the closed state, and moves the door there itself.</summary>
public class OpenDoor : AbstractDoorState
{
    public override void Open()
    {
        Console.WriteLine("Door is already open.");
    }

    public override void Close()
    {
        door.ChangeState(closedState);
    }

    public override bool IsOpen => true;
}
