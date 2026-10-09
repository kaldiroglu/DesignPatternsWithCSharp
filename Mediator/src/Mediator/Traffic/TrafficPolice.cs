namespace dev.kaldiroglu.Mediator.Traffic;

/// <summary>
/// The <b>ConcreteMediator</b>: a traffic police officer who keeps the junction busy or free,
/// and lets one car pass at a time.
/// <para>
/// Every car calls this object from its own thread. The check "is the junction busy?" and
/// the step "make it busy" therefore happen together, inside one lock, so two cars can never
/// both find it free. The car proceeds or waits outside the lock, so a waiting car does not
/// hold up the others.
/// </para>
/// </summary>
public class TrafficPolice : ITrafficMediator
{
    private readonly object gate = new();
    private readonly Junction junction;
    private readonly List<IVehicle> vehicles;

    public TrafficPolice(string name, Junction junction)
    {
        this.junction = junction;
        vehicles = [];
        Console.WriteLine("TrafficPolice " + name + " created.");
    }

    public void Receive(IVehicle vehicle)
    {
        lock (gate)
        {
            vehicle.Stop();
            vehicles.Add(vehicle);
        }
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
        }
        if (granted)
        {
            vehicle.Proceed();
        }
        else
        {
            vehicle.WaitForAWhile();
        }
    }

    public void Done(IVehicle vehicle)
    {
        lock (gate)
        {
            vehicles.Remove(vehicle);
            junction.SetBusy(false);
        }
    }
}
