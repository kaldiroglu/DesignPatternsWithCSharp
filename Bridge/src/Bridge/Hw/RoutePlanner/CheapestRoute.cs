namespace dev.kaldiroglu.Bridge.Hw.RoutePlanner;

/// <summary>A RefinedAbstraction: tolls first, time only as a tie-break.</summary>
public sealed class CheapestRoute : RoutePlanner
{
    public CheapestRoute(IMapProvider maps) : base(maps)
    {
    }

    protected override long Score(Route route) => route.TollMinor * 100_000L + route.Seconds;
}
