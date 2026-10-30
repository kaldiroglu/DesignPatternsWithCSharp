namespace dev.kaldiroglu.TemplateMethod.Task;

/// <summary>The client.</summary>
/// <remarks>
/// A print task ten times with a one-second interval, so
/// <see cref="Run()"/> takes about ten seconds. The demo runner calls <see cref="Run(int)"/>
/// with an interval of 0, which prints the same lines without the waits.
/// </remarks>
public static class Main
{
    public static void Run() => Run(1);

    public static void Run(int interval)
    {
        Task task = new Print(interval, 10);
        task.Run();
    }
}
