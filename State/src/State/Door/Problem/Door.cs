namespace dev.kaldiroglu.State.Door.Problem;

/// <summary>Before the pattern: the door's state is a boolean, and each method has an <c>if</c>.</summary>
public class Door(bool open)
{
    private bool open = open;

    public bool IsOpen => open;

    public void Open()
    {
        if (!open)
            open = true;
        else
            Console.WriteLine("Door is already open.");
    }

    public void Close()
    {
        if (open)
            open = false;
        else
            Console.WriteLine("Door is already closed.");
    }
}
