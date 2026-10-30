namespace dev.kaldiroglu.Mediator.Traffic;

/// <summary>
/// The <b>ConcreteMediator</b>: a traffic police officer who lets one car pass at a time.
/// <para>
/// A car asks once. If the junction is free, the officer tells it to proceed. If not, the
/// officer tells it to stop and keeps it in <c>waiting</c>. When a car is done, the officer
/// tells the first waiting car to proceed, so no car has to ask again, and the junction stays
/// busy while it passes from one car to the next.
/// </para>
/// <para>
/// Every car calls this object from its own thread, so the decisions are made inside one
/// lock. The officer calls the car outside the lock, so a car does not hold up the others.
/// </para>
/// </summary>
public class TrafficPolice : ITrafficMediator
{
    private readonly object gate = new();
    private readonly Junction junction;
    private readonly Queue<IVehicle> waiting;

    public TrafficPolice(string name, Junction junction)
    {
        this.junction = junction;
        waiting = [];
        Console.WriteLine("TrafficPolice " + name + " created.");
    }

    public void AskPermitToPass(IVehicle vehicle)
    {
        bool granted;
        lock (gate)
        {
            granted = !junction.IsBusy();
            if (granted)
            {
                junction.SetBusy(true);
            }
            else
            {
                waiting.Enqueue(vehicle);
            }
        }
        if (granted)
        {
            vehicle.Proceed();
        }
        else
        {
            vehicle.Stop();
        }
    }

    public void Done(IVehicle vehicle)
    {
        IVehicle? next;
        lock (gate)
        {
            if (!waiting.TryDequeue(out next))
            {
                junction.SetBusy(false);
            }
        }
        next?.Proceed();
    }
}
