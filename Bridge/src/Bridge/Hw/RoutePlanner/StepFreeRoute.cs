namespace dev.kaldiroglu.Bridge.Hw.RoutePlanner;

/// <summary>
/// A RefinedAbstraction: fastest, but only among journeys a wheelchair can make.
/// <para>
/// Routes with steps are not filtered out, they are scored beyond reach — so if every candidate
/// has steps the planner still returns the best of a bad set rather than throwing. Whether that
/// is the right call is a good thing to argue about; it is a decision of the abstraction, and no
/// map provider has an opinion on it.
/// </para>
/// </summary>
public sealed class StepFreeRoute : RoutePlanner
{
    public StepFreeRoute(IMapProvider maps) : base(maps)
    {
    }

    protected override long Score(Route route) =>
        route.StepFree ? route.Seconds : int.MaxValue + (long)route.Seconds;
}
