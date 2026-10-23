using dev.kaldiroglu.Command.Account.Domain;

namespace dev.kaldiroglu.Command.Account.Problem;

/// <summary>Stage one: the account remembers only its last move, so a second undo does nothing.</summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Command.Demo -- account-problem-stage-one</c>.
/// </remarks>
public static class StageOneMain
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
    }
}
