namespace dev.kaldiroglu.Mediator.Traffic;

/// <summary>
/// A <b>ConcreteColleague</b>: a car. It knows only the mediator. It asks once, then blocks
/// until the officer tells it to proceed, so it does not poll.
/// </summary>
public class Car : IVehicle
{
    private const int PassingMilliseconds = 100;

    private readonly string name;
    private readonly ITrafficMediator mediator;
    private readonly ManualResetEventSlim permit = new(false);

    public Car(string name, ITrafficMediator mediator)
    {
        this.name = name;
        this.mediator = mediator;
    }

    public void Approach()
    {
        Console.WriteLine("Car " + name + " is approaching the junction.");
    }

    /// <summary>Called by the officer, often from another car's thread. It only releases this car.</summary>
    public void Proceed()
    {
        permit.Set();
    }

    public void Stop()
    {
        Console.WriteLine("Car " + name + " has stopped.");
    }

    /// <summary>What the car's thread does.</summary>
    public void Run()
    {
        Approach();
        Console.WriteLine("Car " + name + " is asking permit to pass the junction.");
        mediator.AskPermitToPass(this);
        permit.Wait();
        Console.WriteLine("Car " + name + " is proceeding through the junction.");
        Thread.Sleep(PassingMilliseconds);
        mediator.Done(this);
    }
}
