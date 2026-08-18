namespace dev.kaldiroglu.Bridge.Hw.RoutePlanner;

/// <summary>A ConcreteImplementor: the routing engine the company built itself.</summary>
public sealed class InHouseMaps : IMapProvider
{
    // from>to                        seconds  toll  stepFree
    private static readonly Dictionary<string, (int Seconds, int Toll, bool StepFree)> Legs = new()
    {
        ["Kadikoy>Levent"] = (2400, 1500, false),
        ["Kadikoy>Uskudar"] = (600, 0, true),
        ["Uskudar>Levent"] = (1500, 900, true),
        ["Kadikoy>Sisli"] = (1800, 700, false),
        ["Sisli>Levent"] = (900, 0, true)
    };

    public string Name => "in-house";

    public int TravelSeconds(string from, string to) => Leg(from, to).Seconds;

    public int TollMinor(string from, string to) => Leg(from, to).Toll;

    public bool StepFree(string from, string to) => Leg(from, to).StepFree;

    private static (int Seconds, int Toll, bool StepFree) Leg(string from, string to) =>
        Legs.TryGetValue($"{from}>{to}", out var found)
            ? found
            : throw new ArgumentException($"no leg {from} > {to}");
}
