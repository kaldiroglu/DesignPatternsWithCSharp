namespace dev.kaldiroglu.Mediator.Hw.AirTraffic;

/// <summary>
/// Homework 2: the <b>Mediator</b> of an airport. Aircraft never talk to each other; they
/// ask the tower for the runway, and the tower gives it to one aircraft at a time.
/// <para>
/// Aircraft may ask from different threads, so every public method takes the same lock: the
/// check "is the runway free?" and the step "give it to this aircraft" happen together.
/// A mediator that many objects share is also a place where threads meet.
/// </para>
/// </summary>
/// <remarks>
/// The Java marks these methods <c>synchronized</c>. Here each one locks a private object,
/// so no code outside the class can take the same lock.
/// </remarks>
public sealed class ControlTower
{
    private readonly object gate = new();
    private readonly Queue<Aircraft> waiting = new();
    private readonly List<string> log = [];
    private Aircraft? onRunway;

    public void Request(Aircraft aircraft)
    {
        lock (gate)
        {
            if (onRunway == null)
            {
                Grant(aircraft);
            }
            else
            {
                waiting.Enqueue(aircraft);
                log.Add(aircraft.CallSign + " waits");
            }
        }
    }

    public void RunwayClear(Aircraft aircraft)
    {
        lock (gate)
        {
            if (onRunway != aircraft)
            {
                throw new InvalidOperationException(aircraft.CallSign + " is not on the runway");
            }
            log.Add(aircraft.CallSign + " clears the runway");
            onRunway = null;
            if (waiting.Count > 0)
            {
                Grant(waiting.Dequeue());
            }
        }
    }

    // Called only with the lock held.
    private void Grant(Aircraft aircraft)
    {
        onRunway = aircraft;
        log.Add(aircraft.CallSign + " may " + aircraft.Intent);
    }

    /// <summary>A copy of the log, as Java's <c>List.copyOf</c> gives.</summary>
    public IReadOnlyList<string> Log
    {
        get
        {
            lock (gate)
            {
                return log.ToList();
            }
        }
    }
}
