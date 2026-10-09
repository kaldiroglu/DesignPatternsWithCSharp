using dev.kaldiroglu.Command.Account.Domain;

namespace dev.kaldiroglu.Command.Account.Lambda;

using dev.kaldiroglu.Command.Account.Solution;
using static dev.kaldiroglu.Command.Account.Lambda.Transactions;

/// <summary>
/// The same story as <c>Account.Solution.Main</c>, with every transaction made of lambdas. The
/// teller and the standing orders are the ones from <c>Solution</c>; they cannot tell the
/// difference. Run it with <c>dotnet run --project src/Command.Demo -- account-lambda</c>.
/// </summary>
public static class Main
{
    public static void Run()
    {
        var deniz = new Domain.Account("Deniz", Money.Of("1000.00"));
        var emre = new Domain.Account("Emre", Money.Of("0.00"));
        var teller = new Teller();

        teller.Perform(Transfer(deniz, emre, Money.Of("300.00")));
        Console.WriteLine("Transfer 300 from Deniz to Emre. Deniz " + deniz.Balance
            + ", Emre " + emre.Balance);
        teller.Undo();
        Console.WriteLine("Undo pressed once. Deniz " + deniz.Balance + ", Emre " + emre.Balance);
        Console.WriteLine("Undo took back the whole transfer.");

        var closed = new Domain.Account("Deniz", Money.Of("750.00"));
        teller.Perform(CloseOut(closed));
        Console.WriteLine("Close out Deniz: balance " + closed.Balance);
        teller.Undo();
        Console.WriteLine("Undo: balance " + closed.Balance
            + " (the command remembered what it took)");

        StandingOrdersExample();
    }

    /// <summary>Three orders are made in the morning. Nothing happens until the night run.</summary>
    private static void StandingOrdersExample()
    {
        Console.WriteLine();
        Console.WriteLine("Standing orders");
        var deniz = new Domain.Account("Deniz", Money.Of("1000.00"));
        var landlord = new Domain.Account("Landlord", Money.Of("0.00"));
        var savings = new Domain.Account("Savings", Money.Of("0.00"));

        var orders = new StandingOrders();
        orders.Schedule(Transfer(deniz, landlord, Money.Of("400.00")));
        orders.Schedule(Transfer(deniz, savings, Money.Of("100.00")));
        orders.Schedule(Deposit(savings, Money.Of("5.00")));
        Console.WriteLine("Morning: " + orders.Pending + " orders waiting. Deniz still has "
            + deniz.Balance);

        var nightRun = new Teller();
        orders.RunThrough(nightRun);
        Console.WriteLine("Night: Deniz " + deniz.Balance + ", Landlord " + landlord.Balance
            + ", Savings " + savings.Balance + ". Orders waiting: " + orders.Pending);
        Console.WriteLine("Night journal: [" + string.Join(", ", nightRun.Journal()) + "]");

        nightRun.Undo();
        Console.WriteLine("Next morning, undo the last order: Savings " + savings.Balance);
    }
}
