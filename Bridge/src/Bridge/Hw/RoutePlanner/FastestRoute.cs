namespace dev.kaldiroglu.Bridge.Hw.RoutePlanner;

/// <summary>A RefinedAbstraction: time is all that matters.</summary>
public sealed class FastestRoute : RoutePlanner
{
    public FastestRoute(IMapProvider maps) : base(maps)
    {
    }

    protected override long Score(Route route) => route.Seconds;
}
