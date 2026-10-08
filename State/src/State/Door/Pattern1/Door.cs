namespace dev.kaldiroglu.State.Door.Pattern1;

/// <summary>
/// The <b>Context</b>. It creates both states and tells each one about the door and about the
/// other state. After that the states change the door's state themselves.
/// </summary>
public class Door
{
    // Set by the constructor through ChangeState.
    private IDoorState state = null!;
    private readonly IDoorState openState = new OpenDoor();
    private readonly IDoorState closedState = new ClosedDoor();

    public Door()
    {
        // Java does this in an instance initializer block, which runs before the constructor body.
        openState.SetDoor(this);
        openState.SetClosedState(closedState);
        closedState.SetDoor(this);
        closedState.SetOpenState(openState);

        ChangeState(closedState);
    }

    public void Open()
    {
        state.Open();
    }

    public void Close()
    {
        state.Close();
    }

    public bool IsOpen => state.IsOpen;

    internal void ChangeState(IDoorState state)
    {
        this.state = state;
    }
}
