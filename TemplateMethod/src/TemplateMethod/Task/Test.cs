namespace dev.kaldiroglu.TemplateMethod.Task;

/// <summary>The client.</summary>
/// <remarks>
/// The Java original's <c>main</c>: a print task ten times with a one-second interval, so
/// <see cref="Run()"/> takes about ten seconds. The demo runner calls <see cref="Run(int)"/>
/// with an interval of 0, which prints the same lines without the waits.
/// </remarks>
public static class Test
{
    public static void Run() => Run(1);

    public static void Run(int interval)
    {
        Task task = new Print("Printing", interval, 10);
        task.Run();
    }
}
