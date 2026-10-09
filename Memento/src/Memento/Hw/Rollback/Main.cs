namespace dev.kaldiroglu.Memento.Hw.Rollback;

/// <summary>
/// Runs two batches of transfers to Can. The first fails at Ayse's transfer, and every
/// account gets its balance back; the second succeeds.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Memento.Demo -- hw-rollback</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        Account ali = new Account("Ali", 100);
        Account ayse = new Account("Ayse", 50);
        Account can = new Account("Can", 0);
        List<Account> accounts = [ali, ayse, can];

        Batch failing = new Batch();
        failing.Add(ali, can, 80);
        failing.Add(ayse, can, 70);
        Console.WriteLine("Batch 1: " + failing.Run(accounts) + " -> " + ali + ", " + ayse + ", " + can);

        Batch working = new Batch();
        working.Add(ali, can, 80);
        working.Add(ayse, can, 50);
        Console.WriteLine("Batch 2: " + working.Run(accounts) + " -> " + ali + ", " + ayse + ", " + can);
    }
}
