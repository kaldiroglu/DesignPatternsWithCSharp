namespace dev.kaldiroglu.Mediator.Hw.AirTraffic;

/// <summary>
/// Three aircraft ask the tower for the runway. The tower gives it to one at a time and
/// calls the next one when the runway is clear.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Mediator.Demo -- hw-airtraffic</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        ControlTower tower = new ControlTower();
        Aircraft tk1 = new Aircraft("TK1", "land", tower);
        Aircraft pc2 = new Aircraft("PC2", "land", tower);
        Aircraft aj3 = new Aircraft("AJ3", "take off", tower);
        tk1.Request();
        pc2.Request();
        aj3.Request();
        tk1.Clear();
        pc2.Clear();
        foreach (string line in tower.Log)
        {
            Console.WriteLine(line);
        }
    }
}
