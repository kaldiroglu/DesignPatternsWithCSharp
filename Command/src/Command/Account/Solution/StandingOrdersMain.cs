using dev.kaldiroglu.Command.Account.Domain;

namespace dev.kaldiroglu.Command.Account.Solution;

/// <summary>Three orders are made in the morning. Nothing happens until the night run.</summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Command.Demo -- account-standing-orders</c>.
/// </remarks>
public static class StandingOrdersMain
{
    public static void Run()
    {
        Console.WriteLine("Standing orders");
        var deniz = new Domain.Account("Deniz", Money.Of("1000.00"));
        var landlord = new Domain.Account("Landlord", Money.Of("0.00"));
        var savings = new Domain.Account("Savings", Money.Of("0.00"));

        var orders = new StandingOrders();
        orders.Schedule(new Transfer(deniz, landlord, Money.Of("400.00")));
        orders.Schedule(new Transfer(deniz, savings, Money.Of("100.00")));
        orders.Schedule(new Deposit(savings, Money.Of("5.00")));
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
