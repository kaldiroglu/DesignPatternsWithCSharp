namespace dev.kaldiroglu.Mediator.Traffic;

/// <summary>
/// The <b>Colleague</b>: a vehicle at the junction.
/// </summary>
public interface IVehicle
{
    void Approach();

    void Proceed();

    void Stop();
}
