namespace dev.kaldiroglu.State.Door.Pattern1;

/// <summary>
/// The <b>Context</b>. It forwards each request to its state, and the state changes it.
/// Every door makes its own two state objects, because each state holds its door.
/// </summary>
public class Door
{
    private readonly AbstractDoorState openState = new OpenDoor();
    private readonly AbstractDoorState closedState = new ClosedDoor();
    private IDoorState state;

    public Door()
    {
        openState.Wire(this, openState, closedState);
        closedState.Wire(this, openState, closedState);
        state = closedState;
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
