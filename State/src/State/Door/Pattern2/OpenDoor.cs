namespace dev.kaldiroglu.State.Door.Pattern2;

/// <summary>The open state. It knows no other state and does not change the door.</summary>
public class OpenDoor : IDoorState
{
    public void Open()
    {
        Console.WriteLine("Door is already open.");
    }

    public void Close()
    {
    }

    public bool IsOpen => true;
}
