using dev.kaldiroglu.Command.Account.Domain;

namespace dev.kaldiroglu.Command.Account.Problem;

/// <summary>Runs the three stages, and shows the stage-three teller undoing only half of a transfer.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Command.Demo -- account-problem</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var one = new OneStepAccount("Deniz", Money.Of("1000.00"));
        one.Deposit(Money.Of("500.00"));
        one.Withdraw(Money.Of("200.00"));
        one.Undo();
        one.Undo();
        Console.WriteLine("Stage one: deposit 500, withdraw 200, undo twice. Balance: "
            + one.Balance + " (the second undo did nothing)");

        var two = new HistoryAccount("Deniz", Money.Of("1000.00"));
        two.Deposit(Money.Of("500.00"));
        two.Withdraw(Money.Of("200.00"));
        two.Undo();
        two.Undo();
        Console.WriteLine("Stage two: the same steps. Balance: " + two.Balance);
        Console.WriteLine("Stage two journal: " + Show(two.Journal()));

        var deniz = new Domain.Account("Deniz", Money.Of("1000.00"));
        var emre = new Domain.Account("Emre", Money.Of("0.00"));
        var teller = new Teller();
        teller.Transfer(deniz, emre, Money.Of("300.00"));
        Console.WriteLine("Stage three: transfer 300 from Deniz to Emre. Deniz "
            + deniz.Balance + ", Emre " + emre.Balance);
        teller.Undo();
        Console.WriteLine("Undo pressed once. Deniz " + deniz.Balance + ", Emre " + emre.Balance);
        Console.WriteLine("Undo took back only the deposit. "
            + Money.Of("1000.00").Minus(deniz.Balance.Plus(emre.Balance))
            + " lira left Deniz and arrived nowhere.");
        Console.WriteLine("Journal: " + Show(teller.Journal()));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
