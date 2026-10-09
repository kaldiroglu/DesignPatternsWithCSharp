namespace dev.kaldiroglu.Mediator.Traffic;

/// <summary>
/// The <b>ConcreteMediator</b>: a traffic police officer who keeps the junction busy or free,
/// and lets one car pass at a time.
/// </summary>
public class TrafficPolice : ITrafficMediator
{
    // NOTE: name is set here and never read again; the constructor prints the parameter. The
    // Java field is never read either.
    private readonly string name;
    private readonly Junction junction;
    private readonly List<IVehicle> vehicles;

    public TrafficPolice(string name, Junction junction)
    {
        this.name = name;
        this.junction = junction;
        vehicles = [];
        Console.WriteLine("TrafficPolice " + name + " created.");
    }

    public void Receive(IVehicle vehicle)
    {
        vehicle.Stop();
        // NOTE: List is not safe for threads, and nothing locks it. Here every car is
        // received on the main thread, but Done removes cars from it on the cars' own
        // threads, at the same time. The Java ArrayList has the same behavior.
        vehicles.Add(vehicle);
    }

    public void AskPermitToPass(IVehicle vehicle)
    {
        // NOTE: a race condition. The check IsBusy() and the step SetBusy(true) are two
        // separate steps with no lock around them. Two cars can both see a free junction
        // before either sets it busy, and then both pass at the same time. The Java has the
        // same behavior. A lock around the check and the step, as ControlTower in
        // Hw.AirTraffic has, would fix it.
        if (!junction.IsBusy())
        {
            junction.SetBusy(true);
            vehicle.Proceed();
        }
        else
        {
            vehicle.WaitForAWhile();
        }
    }

    public void Done(IVehicle vehicle)
    {
        vehicles.Remove(vehicle);
        junction.SetBusy(false);
    }
}
