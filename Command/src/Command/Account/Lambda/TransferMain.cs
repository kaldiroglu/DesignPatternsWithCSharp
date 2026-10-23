using dev.kaldiroglu.Command.Account.Domain;

namespace dev.kaldiroglu.Command.Account.Lambda;

using dev.kaldiroglu.Command.Account.Solution;
using static dev.kaldiroglu.Command.Account.Lambda.Transactions;

/// <summary>
/// The transfer from the problem again, made of lambdas. The teller is the one from
/// <c>Solution</c>, and one undo takes back the whole transfer.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Command.Demo -- account-lambda-transfer</c>.
/// </remarks>
public static class TransferMain
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
    }
}
