namespace dev.kaldiroglu.Bridge.Hw.RoutePlanner;

/// <summary>
/// The Abstraction: how to choose between journeys.
/// <para>
/// It builds the candidates and asks each subclass to score them. The candidate-building is
/// shared, the preference is not — which is the split that makes three route kinds three small
/// classes rather than three copies of this method.
/// </para>
/// </summary>
public abstract class RoutePlanner
{
    protected readonly IMapProvider Maps;

    protected RoutePlanner(IMapProvider maps) =>
        Maps = maps ?? throw new ArgumentNullException(nameof(maps));

    public Route Plan(string from, string to, IReadOnlyList<string> hubs)
    {
        var candidates = new List<Route> { Measure([from, to]) };
        foreach (var hub in hubs)
        {
            candidates.Add(Measure([from, hub, to]));
        }

        return candidates.MinBy(Score)!;
    }

    /// <summary>Lower is better.</summary>
    protected abstract long Score(Route route);

    private Route Measure(IReadOnlyList<string> stops)
    {
        var seconds = 0;
        var toll = 0;
        var stepFree = true;
        for (var i = 0; i < stops.Count - 1; i++)
        {
            var a = stops[i];
            var b = stops[i + 1];
            seconds += Maps.TravelSeconds(a, b);
            toll += Maps.TollMinor(a, b);
            stepFree &= Maps.StepFree(a, b);
        }

        return new Route(stops, seconds, toll, stepFree);
    }
}
