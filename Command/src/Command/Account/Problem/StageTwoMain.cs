using dev.kaldiroglu.Command.Account.Domain;

namespace dev.kaldiroglu.Command.Account.Problem;

/// <summary>Stage two: the account keeps a history, so both steps can be undone.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Command.Demo -- account-problem-stage-two</c>.
/// </remarks>
public static class StageTwoMain
{
    public static void Run()
    {
        var two = new HistoryAccount("Deniz", Money.Of("1000.00"));
        two.Deposit(Money.Of("500.00"));
        two.Withdraw(Money.Of("200.00"));
        two.Undo();
        two.Undo();
        Console.WriteLine("Stage two: the same steps. Balance: " + two.Balance);
        Console.WriteLine("Stage two journal: " + Show(two.Journal()));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
