namespace dev.kaldiroglu.State.Hw.AirConditioner;

/// <summary>
/// Homework 1: an air conditioner's states.
/// <para>
/// Off, Idle, Cooling and Heating. The user turns it on and off and sets a target; a sensor
/// reports the room temperature. Which state comes next depends on the room and the target,
/// so the states decide it. The context only forwards and keeps the temperatures.
/// </para>
/// </summary>
public sealed class AirConditioner
{
    private AcState state = AcState.OffState();
    private int target = 22;
    private int room;
    private readonly List<string> log = [];

    public AirConditioner(int room)
    {
        this.room = room;
    }

    public void PowerOn()
    {
        state = state.PowerOn(this);
    }

    public void PowerOff()
    {
        state = state.PowerOff(this);
    }

    public void SetTarget(int target)
    {
        this.target = target;
        state = state.Changed(this);
    }

    /// <summary>The sensor reports a new room temperature.</summary>
    public void RoomIs(int room)
    {
        this.room = room;
        state = state.Changed(this);
    }

    public string State => state.Name;

    internal int Target => target;

    internal int Room => room;

    internal void Record(string line)
    {
        log.Add(line);
    }

    public IReadOnlyList<string> Log => log.ToList().AsReadOnly();
}
