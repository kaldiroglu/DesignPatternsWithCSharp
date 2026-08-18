namespace dev.kaldiroglu.Bridge.Hw.RoutePlanner;

/// <summary>
/// A ConcreteImplementor: the vendor the company switched to.
/// <para>
/// Its numbers disagree with the in-house engine's — better traffic data, different toll records,
/// and a more careful survey of which stations have lifts. That disagreement is the point: the
/// routing rules above must produce a different answer without a different line of code.
/// </para>
/// </summary>
public sealed class VendorMaps : IMapProvider
{
    private static readonly Dictionary<string, (int Seconds, int Toll, bool StepFree)> Legs = new()
    {
        ["Kadikoy>Levent"] = (2100, 1500, false),
        ["Kadikoy>Uskudar"] = (540, 0, true),
        ["Uskudar>Levent"] = (1080, 900, false),   // the vendor knows about the steps
        ["Kadikoy>Sisli"] = (1500, 700, true),
        ["Sisli>Levent"] = (1020, 0, true)
    };

    public string Name => "vendor";

    public int TravelSeconds(string from, string to) => Leg(from, to).Seconds;

    public int TollMinor(string from, string to) => Leg(from, to).Toll;

    public bool StepFree(string from, string to) => Leg(from, to).StepFree;

    private static (int Seconds, int Toll, bool StepFree) Leg(string from, string to) =>
        Legs.TryGetValue($"{from}>{to}", out var found)
            ? found
            : throw new ArgumentException($"no leg {from} > {to}");
}
