namespace dev.kaldiroglu.State.Door.Pattern2;

/// <summary>
/// The <b>Context</b>. It passes each request to its state, which does the work, and then asks
/// the manager for the next state.
/// </summary>
public class Door
{
    private readonly DoorStateManager manager = new();
    private IDoorState state;

    public Door()
    {
        state = manager.Initial();
    }

    public void Open()
    {
        state.Open();
        state = manager.Next(state, Request.Open);
    }

    public void Close()
    {
        state.Close();
        state = manager.Next(state, Request.Close);
    }

    public bool IsOpen => state.IsOpen;
}
