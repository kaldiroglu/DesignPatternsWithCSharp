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
    // NOTE: moving is set in the constructor and in Stop(), and never read. The Java field is
    // never read either.
    private bool moving;
    private readonly Junction junction;
    private readonly ITrafficMediator mediator;
    private readonly Thread thread;

    public Car(string name, Junction junction, ITrafficMediator mediator, bool moving)
    {
        thread = new Thread(Run) { Name = name };
        this.junction = junction;
        this.mediator = mediator;
        this.moving = moving;
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
        Console.WriteLine("Car " + Name + " is approaching to junction " + junction.Name);
    }

    public void Proceed()
    {
        Console.WriteLine("Car " + Name + " is proceeding through junction " + junction.Name);
        mediator.Done(this);
    }

    public void Stop()
    {
        moving = false;
        Console.WriteLine("Car " + Name + " has stopped.");
    }

    public void WaitForAWhile()
    {
        Console.WriteLine("Car " + Name + " is waiting.");
        try
        {
            Thread.Sleep(1000);
        }
        catch (ThreadInterruptedException e)
        {
            Console.Error.WriteLine(e);
        }
        // NOTE: the car asks again by calling the mediator, and the mediator may call
        // WaitForAWhile again. This is recursion, not a loop: each wait adds two frames to the
        // stack. The Java has the same behavior.
        mediator.AskPermitToPass(this);
    }

    /// <summary>What the car's thread does: ask the mediator for permission to pass.</summary>
    public void Run()
    {
        Console.WriteLine("Car " + Name + " is asking permit to pass junction " + junction.Name);
        mediator.AskPermitToPass(this);
    }
}
