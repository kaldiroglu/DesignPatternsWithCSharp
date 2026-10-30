namespace dev.kaldiroglu.Mediator.Traffic;

/// <summary>
/// The <b>Mediator</b>: cars talk to it, never to each other.
/// </summary>
public interface ITrafficMediator
{
    void AskPermitToPass(IVehicle vehicle);

    void Done(IVehicle vehicle);
}
