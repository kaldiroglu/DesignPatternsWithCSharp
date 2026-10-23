using dev.kaldiroglu.Command.Account.Domain;

namespace dev.kaldiroglu.Command.Account.Lambda;

using dev.kaldiroglu.Command.Account.Solution;
using static dev.kaldiroglu.Command.Account.Lambda.Transactions;

/// <summary>Close out an account, then undo: the command gives back what it took.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Command.Demo -- account-lambda-close-out</c>.
/// </remarks>
public static class CloseOutMain
{
    public static void Run()
    {
        var closed = new Domain.Account("Deniz", Money.Of("750.00"));
        var teller = new Teller();
        teller.Perform(CloseOut(closed));
        Console.WriteLine("Close out Deniz: balance " + closed.Balance);
        teller.Undo();
        Console.WriteLine("Undo: balance " + closed.Balance
            + " (the command remembered what it took)");
    }
}
