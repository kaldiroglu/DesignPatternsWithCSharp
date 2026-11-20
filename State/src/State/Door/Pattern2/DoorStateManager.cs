namespace dev.kaldiroglu.State.Door.Pattern2;

/// <summary>
/// The one place that decides the next state. Its table maps a state and a request to the next
/// state; a request with no line keeps the state. A new state or a new request is new lines here,
/// and the existing states do not change.
/// </summary>
public class DoorStateManager
{
    private readonly IDoorState openState = new OpenDoor();
    private readonly IDoorState closedState = new ClosedDoor();
    private readonly Dictionary<(IDoorState, Request), IDoorState> next;

    public DoorStateManager()
    {
        next = new()
        {
            [(closedState, Request.Open)] = openState,
            [(openState, Request.Close)] = closedState,
        };
    }

    public IDoorState Initial() => closedState;

    public IDoorState Next(IDoorState current, Request request) =>
        next.GetValueOrDefault((current, request), current);
}
