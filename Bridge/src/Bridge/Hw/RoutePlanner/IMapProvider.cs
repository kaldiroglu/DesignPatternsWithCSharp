namespace dev.kaldiroglu.Bridge.Hw.RoutePlanner;

/// <summary>
/// The Implementor: what the routing rules are allowed to ask of a map.
/// <para>
/// Every primitive here is a measurement of one leg. None of them is a routing decision, and
/// none of them mentions a vendor. That is the property the exercise is checking: if a single
/// vendor type name reaches the abstraction, swapping vendors will touch the routing rules, and
/// the whole point of the solution is lost.
/// </para>
/// </summary>
public interface IMapProvider
{
    string Name { get; }

    int TravelSeconds(string from, string to);

    /// <summary>Tolls in minor units, so there is no rounding argument.</summary>
    int TollMinor(string from, string to);

    bool StepFree(string from, string to);
}
