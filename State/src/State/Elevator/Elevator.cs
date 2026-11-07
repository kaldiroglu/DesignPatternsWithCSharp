namespace dev.kaldiroglu.State.Elevator;

/// <summary>
/// The <b>Context</b>. It forwards every request to the current state. The states never
/// change the state; the caller does, with <see cref="SetState"/>. That makes this example
/// closer to Strategy than to State.
/// </summary>
public class Elevator(IElevatorState state)
{
    private IElevatorState state = state;

    public IElevatorState State => state;

    public void SetState(IElevatorState state)
    {
        this.state = state;
    }

    public void GoDown()
    {
        state.GoDown();
    }

    public void GoUp()
    {
        state.GoUp();
    }

    public void OpenDoor()
    {
        state.OpenDoor();
    }

    public void Stop()
    {
        state.Stop();
    }
}
