namespace dev.kaldiroglu.State.Elevator;

/// <summary>A <b>ConcreteState</b>: the elevator is going up.</summary>
public class GoingUpState : IElevatorState
{
    public void GoDown()
    {
        Console.WriteLine("Please first stop the elevator.");
    }

    public void GoUp()
    {
        Console.WriteLine(":)");
    }

    public void OpenDoor()
    {
        Console.WriteLine("Please first stop the elevator.");
    }

    public void Stop()
    {
        // "Stooping!" is the Java output, kept as it is so that the two outputs are the same.
        Console.WriteLine("Stooping!");
    }
}
