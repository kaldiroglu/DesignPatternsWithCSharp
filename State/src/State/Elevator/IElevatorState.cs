namespace dev.kaldiroglu.State.Elevator;

/// <summary>The <b>State</b>: the four requests an elevator answers.</summary>
public interface IElevatorState
{
    void GoUp();

    void GoDown();

    void Stop();

    void OpenDoor();
}
