namespace dev.kaldiroglu.Mediator.Traffic;

/// <summary>
/// The <b>Mediator</b>: cars talk to it, never to each other.
/// </summary>
public interface ITrafficMediator
{
    void Receive(IVehicle vehicle);

    void AskPermitToPass(IVehicle vehicle);

    void Done(IVehicle vehicle);
}
