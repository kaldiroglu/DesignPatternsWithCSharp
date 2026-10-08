namespace dev.kaldiroglu.State.Elevator;

/// <summary>Sends requests to the elevator in three states. The Java original's <c>main</c>.</summary>
public static class Test
{
    public static void Run()
    {
        Elevator elevator = new Elevator(new StoppedState());
        elevator.OpenDoor();

        elevator.SetState(new GoingUpState());
        elevator.OpenDoor();
        elevator.GoUp();
        elevator.GoDown();

        elevator.SetState(new StoppedState());
        elevator.OpenDoor();
        elevator.SetState(new GoingDownState());
        elevator.GoDown();
    }
}
