namespace dev.kaldiroglu.Mediator.Traffic;

/// <summary>
/// A <b>ConcreteColleague</b>: a car. Each car runs on its own thread.
/// </summary>
/// <remarks>
/// In the Java, <c>Car</c> extends <c>Thread</c>. In C# <c>Thread</c> is <c>sealed</c>, so a
/// car holds a thread instead. The thread is created in the constructor with the car's name,
/// and <see cref="Start"/> starts it; the thread then calls <see cref="Run"/>, as Java's
/// <c>Thread.start()</c> calls <c>run()</c>.
/// </remarks>
public class Car : IVehicle
{
    private readonly Junction junction;
    private readonly ITrafficMediator mediator;
    private readonly Thread thread;
    private bool waiting;

    public Car(string name, Junction junction, ITrafficMediator mediator)
    {
        thread = new Thread(Run) { Name = name };
        this.junction = junction;
        this.mediator = mediator;
        Approach();
        mediator.Receive(this);
    }

    /// <summary>The car's name, which is also its thread's name, as <c>getName()</c> is in Java.</summary>
    public string Name => thread.Name!;

    /// <summary>Starts the car's thread, which calls <see cref="Run"/>.</summary>
    public void Start()
    {
        thread.Start();
    }

    public void Approach()
    {
        Console.WriteLine("Car " + Name + " is approaching junction " + junction.Name);
    }

    public void Proceed()
    {
        Console.WriteLine("Car " + Name + " is proceeding through junction " + junction.Name);
        mediator.Done(this);
    }

    public void Stop()
    {
        Console.WriteLine("Car " + Name + " has stopped.");
    }

    /// <summary>Waits, then lets <see cref="Run"/> ask again. It does not call the mediator itself.</summary>
    public void WaitForAWhile()
    {
        Console.WriteLine("Car " + Name + " is waiting.");
        waiting = true;
        try
        {
            Thread.Sleep(1000);
        }
        catch (ThreadInterruptedException)
        {
            // The car was interrupted: it simply asks again.
        }
    }

    /// <summary>What the car's thread does: ask the mediator until it may pass.</summary>
    public void Run()
    {
        Console.WriteLine("Car " + Name + " is asking permit to pass junction " + junction.Name);
        do
        {
            waiting = false;
            mediator.AskPermitToPass(this);
        } while (waiting);
    }
}
