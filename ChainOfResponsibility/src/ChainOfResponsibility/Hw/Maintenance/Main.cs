namespace dev.kaldiroglu.ChainOfResponsibility.Hw.Maintenance;

/// <summary>
/// Puts five requests in the queue and processes them. The queue knows only the first
/// developer; each request goes along the chain to the one who takes it.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/ChainOfResponsibility.Demo -- hw-maintenance</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        Developer first = new BugFixer("Ali");
        first.Then(new UiDesigner("Zeynep"))
                .Then(new FeatureDeveloper("Kerem"))
                .Then(new ProjectTeam("the project team"));

        RequestQueue queue = new RequestQueue(first);
        queue.Add(new Request(RequestKind.BUG, "Login fails", 1));
        queue.Add(new Request(RequestKind.IMPROVEMENT, "Faster search", 4));
        queue.Add(new Request(RequestKind.UI_CHANGE, "New logo", 2));
        queue.Add(new Request(RequestKind.IMPROVEMENT, "New report engine", 30));
        queue.Add(new Request(RequestKind.PROJECT, "Mobile app", 120));

        foreach (string assignment in queue.ProcessAll())
        {
            Console.WriteLine(assignment);
        }
    }
}
