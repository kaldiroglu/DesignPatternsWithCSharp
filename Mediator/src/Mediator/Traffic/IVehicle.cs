namespace dev.kaldiroglu.Mediator.Traffic;

/// <summary>
/// The <b>Colleague</b>: a vehicle at the junction.
/// </summary>
/// <remarks>
/// The Java names the stop method <c>stopp()</c>, because a <c>Car</c> there extends
/// <c>Thread</c>, and <c>Thread.stop()</c> is <c>final</c> and cannot be declared again. In C#
/// a car does not extend <c>Thread</c>, so the method is named <c>Stop()</c>.
/// </remarks>
public interface IVehicle
{
    void Approach();

    void Proceed();

    void Stop();

    void WaitForAWhile();
}
