namespace dev.kaldiroglu.Bridge.Hw.RoutePlanner;

/// <summary>One candidate journey, already measured.</summary>
public record Route(IReadOnlyList<string> Stops, int Seconds, int TollMinor, bool StepFree)
{
    public string Describe() =>
        string.Join(" > ", Stops)
        + $"  [{Seconds}s, {TollMinor} toll, {(StepFree ? "step-free" : "steps")}]";
}
