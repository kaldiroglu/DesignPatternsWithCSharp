namespace dev.kaldiroglu.State.Elevator;

/// <summary>
/// One trip up and one trip down. After each request, the caller sets the next state: the
/// states only print what the elevator does.
/// </summary>
public static class Main
{
    public static void Run()
    {
        Elevator elevator = new Elevator(new StoppedState());
        elevator.OpenDoor();

        elevator.GoUp();
        elevator.SetState(new GoingUpState());
        elevator.OpenDoor();
        elevator.GoDown();

        elevator.Stop();
        elevator.SetState(new StoppedState());
        elevator.OpenDoor();

        elevator.GoDown();
        elevator.SetState(new GoingDownState());
        elevator.GoDown();
        elevator.Stop();
        elevator.SetState(new StoppedState());
    }
}
