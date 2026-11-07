namespace dev.kaldiroglu.State.Elevator;

/// <summary>A <b>ConcreteState</b>: the elevator is stopped. It may open its door; the caller decides when it starts to move.</summary>
public class StoppedState : IElevatorState
{
    public void GoDown()
    {
        Console.WriteLine("Starting to go down.");
    }

    public void GoUp()
    {
        Console.WriteLine("Starting to go up.");
    }

    public void OpenDoor()
    {
        Console.WriteLine("Door is open.");
    }

    public void Stop()
    {
        Console.WriteLine("Already stopped.");
    }
}
