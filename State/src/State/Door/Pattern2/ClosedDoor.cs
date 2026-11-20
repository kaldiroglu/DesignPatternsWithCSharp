namespace dev.kaldiroglu.State.Door.Pattern2;

/// <summary>The closed state. It knows no other state and does not change the door.</summary>
public class ClosedDoor : IDoorState
{
    public void Open()
    {
    }

    public void Close()
    {
        Console.WriteLine("Door is already closed.");
    }

    public bool IsOpen => false;
}
