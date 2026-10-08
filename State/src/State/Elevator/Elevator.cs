namespace dev.kaldiroglu.State.Elevator;

/// <summary>
/// The <b>Context</b>. It implements the same interface as its states and forwards every
/// request to the current one. Here the caller changes the state with <see cref="SetState"/>.
/// </summary>
public class Elevator(IElevatorState state) : IElevatorState
{
    private IElevatorState state = state;

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
