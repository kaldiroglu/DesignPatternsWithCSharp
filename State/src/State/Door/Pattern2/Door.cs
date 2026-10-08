namespace dev.kaldiroglu.State.Door.Pattern2;

/// <summary>
/// The <b>Context</b>. It creates the <see cref="DoorStateManager"/>, and the manager sets
/// its state.
/// </summary>
public class Door
{
    // Set by the manager in the constructor.
    private IDoorState state = null!;
    internal DoorStateManager dsm;

    public Door()
    {
        dsm = new DoorStateManager(this);
        dsm.CloseDoor();
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
