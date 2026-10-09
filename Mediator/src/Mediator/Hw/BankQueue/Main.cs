namespace dev.kaldiroglu.Mediator.Hw.BankQueue;

/// <summary>
/// Two tellers and three customers meet only through the queue manager, which pairs them
/// in arrival order.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Mediator.Demo -- hw-bankqueue</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        QueueManager manager = new QueueManager();
        Teller first = new Teller("Teller 1", manager);
        Teller second = new Teller("Teller 2", manager);
        Customer ayse = new Customer("Ayse", manager);
        Customer mert = new Customer("Mert", manager);
        Customer deniz = new Customer("Deniz", manager);
        first.Free();
        ayse.Arrive();
        mert.Arrive();
        deniz.Arrive();
        second.Free();
        first.Free();
        foreach (string line in manager.Log)
        {
            Console.WriteLine(line);
        }
    }
}
