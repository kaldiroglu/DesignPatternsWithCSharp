namespace dev.kaldiroglu.State.Elevator;

/// <summary>A <b>ConcreteState</b>: the elevator is stopped. It may go up, go down or open its door.</summary>
public class StoppedState : IElevatorState
{
    public void GoDown()
    {
        Console.WriteLine("Going down.");
    }

    public void GoUp()
    {
        Console.WriteLine("Going up.");
    }

    public void OpenDoor()
    {
        Console.WriteLine("Door is open.");
    }

    public void Stop()
    {
        Console.WriteLine(":)");
    }
}
