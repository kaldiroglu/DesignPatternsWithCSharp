namespace dev.kaldiroglu.Mediator.Traffic;

/// <summary>
/// The <b>Colleague</b>: a vehicle at the junction.
/// </summary>
/// <remarks>
/// A car holds a thread rather than being one, so the stop method can be named
/// <c>Stop()</c>.
/// </remarks>
public interface IVehicle
{
    void Approach();

    void Proceed();

    void Stop();

    void WaitForAWhile();
}
